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
    public void Move(float dir)
    {
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
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
        rb.linearVelocity = new Vector2(dir * (moveSpeed * dashSpeed), 0f);
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

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
