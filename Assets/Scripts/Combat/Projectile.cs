using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private LayerMask targetLayers;
    private Transform owner;
    private int damage;
    private float lifeTimer;
    private bool released;
    private Action<Projectile> releaseToPool;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    public void Launch(Vector2 position, bool facingRight, AttackData data,
                       LayerMask targets, Transform owner, Action<Projectile> release)
    {
        this.owner = owner;
        targetLayers = targets;
        damage = data.damage;
        lifeTimer = data.projectileLifetime;
        releaseToPool = release;
        released = false;

        transform.position = position;

        // lật hình theo hướng bắn
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (facingRight ? 1f : -1f);
        transform.localScale = s;

        rb.linearVelocity = (facingRight ? Vector2.right : Vector2.left) * data.projectileSpeed;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) Release();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (released) return;
        if (((1 << col.gameObject.layer) & targetLayers) == 0) return;
        if (col.transform.IsChildOf(owner)) return;

        IDamageable receiver = col.GetComponentInParent<IDamageable>();
        if (receiver == null) return;

        // truyền chính projectile làm source để việc xác định hướng đòn khi đỡ dựa vào vị trí đạn
        receiver.TakeDamage(damage, transform);
        Release();
    }

    private void Release()
    {
        if (released) return;   // tránh trả về pool 2 lần
        released = true;
        rb.linearVelocity = Vector2.zero;
        releaseToPool(this);
    }
}