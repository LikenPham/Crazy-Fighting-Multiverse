using UnityEngine;

public class MovementModule : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool facingRight = true;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float fallGravityMultiplier = 2.5f;
    [SerializeField] private float jumpGravityMultiplier = 1.5f;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private float skinWidth = 0.05f;

    private LayerMask blockLayers;                       // layer của đối thủ
    private readonly RaycastHit2D[] castHits = new RaycastHit2D[8];
    private ContactFilter2D castFilter;

    private float defaultGravity;
    private bool isDashing;
    private bool gravityLocked;

    public float VerticalVelocity => rb.linearVelocity.y;
    public bool FacingRight => facingRight;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetBlockLayers(LayerMask mask)
    {
        blockLayers = mask;
        castFilter = new ContactFilter2D();
        castFilter.SetLayerMask(mask);
        castFilter.useTriggers = false;   // bỏ qua hitbox/hurtbox dạng trigger
    }

    // Trả về quãng đường tối đa được đi theo hướng dir (>= 0)
    private float AllowedDistance(float dir, float wanted)
    {
        if (dir == 0f || wanted <= 0f) return wanted;

        Vector2 direction = new Vector2(Mathf.Sign(dir), 0f);
        int count = bodyCollider.Cast(direction, castFilter, castHits, wanted + skinWidth);

        float allowed = wanted;
        for (int i = 0; i < count; i++)
        {
            RaycastHit2D h = castHits[i];

            // chỉ tính va chạm ngang, bỏ qua đứng trên đầu / dưới chân
            if (Mathf.Abs(h.normal.x) < 0.5f) continue;

            // normal hướng ngược chiều đi mới là đang đi vào đối phương
            // nếu đang chồng nhau mà đi ra xa thì không chặn
            if (h.normal.x * direction.x >= 0f) continue;

            allowed = Mathf.Min(allowed, Mathf.Max(0f, h.distance - skinWidth));
        }
        return allowed;
    }

    public void Move(float dir)
    {
        float wanted = Mathf.Abs(dir) * moveSpeed * Time.fixedDeltaTime;
        float allowed = AllowedDistance(dir, wanted);

        float vx = Mathf.Sign(dir) * allowed / Time.fixedDeltaTime;
        if (dir == 0f) vx = 0f;

        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
        FaceDirection(dir);
    }

    public void Stop()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public bool IsGrounded()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return hit != null;
    }

    public void ApplyBetterGravity()
    {
        if (isDashing || gravityLocked) return;

        float velocityY = rb.linearVelocity.y;

        if (velocityY < 0f)
            rb.gravityScale = defaultGravity * fallGravityMultiplier;
        else if (velocityY > 0f)
            rb.gravityScale = defaultGravity * jumpGravityMultiplier;
        else
            rb.gravityScale = defaultGravity;

        if (velocityY < -maxFallSpeed)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxFallSpeed);
    }

    public void LockInAir()
    {
        gravityLocked = true;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
    }

    public void UnlockGravity()
    {
        gravityLocked = false;
        rb.gravityScale = defaultGravity;
    }

    public void Dash()
    {
        isDashing = true;
        float dir = facingRight ? 1f : -1f;
        rb.gravityScale = 0f;

        float speed = moveSpeed * dashSpeed;
        float allowed = AllowedDistance(dir, speed * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(dir * allowed / Time.fixedDeltaTime, 0f);
    }

    public void EndDash()
    {
        isDashing = false;
        rb.gravityScale = defaultGravity;
        rb.linearVelocity = new Vector2(0f,0f);

    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;

        Physics2D.SyncTransforms();
        Depenetrate();
    }

    private void Depenetrate()
    {
        Collider2D[] overlaps = new Collider2D[4];
        int count = bodyCollider.Overlap(castFilter, overlaps);

        for (int i = 0; i < count; i++)
        {
            ColliderDistance2D d = bodyCollider.Distance(overlaps[i]);
            if (!d.isOverlapped) continue;

            // d.normal hướng từ mình ra ngoài, distance âm = độ sâu chồng
            Vector2 push = d.normal * (d.distance - skinWidth);   // d.distance < 0 nên push ngược về phía thoát ra
            push.y = 0f;
            rb.position += push;
        }
    }

    private void FaceDirection(float dir)
    {
        if (dir > 0f && !facingRight)
        {
            Flip();
        }
        else if(dir < 0f && facingRight)
        {
            Flip();
        }
    }

    public void SetFacing(bool right)
    {
        if (facingRight != right) Flip();
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
