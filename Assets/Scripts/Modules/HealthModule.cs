using System;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class HealthModule : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private MovementModule movementModule;
    [SerializeField, Range(0f, 1f)] private float blockDamage = 0.1f;

    private int currentHealth;
    private bool isDead;

    public event Action Died;
    public event Action Hurt;

    public bool isBlocking { get; set;}
    public bool isInvinciblle {  get; set;}

    private void Awake()
    {
        currentHealth = maxHealth;
        if (movementModule == null) movementModule = GetComponent<MovementModule>();
    }

    public void TakeDamage(int damage, Transform source)
    {
        Debug.Log($"{name} TakeDamage {damage} (dead={isDead}, invincible={isInvinciblle}, hp={currentHealth})");

        if (isDead || isInvinciblle) return;

        bool blocked = isBlocking && IsFrontal(source);
        if (blocked)
            damage = Mathf.RoundToInt(damage * blockDamage);

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (!blocked)
            Hurt?.Invoke();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Died?.Invoke();
    }

    private bool IsFrontal(Transform source)
    {
        if (source == null || movementModule == null) return false;

        float dx = source.position.x - transform.position.x;
        return dx >= 0f ? movementModule.FacingRight : !movementModule.FacingRight;
    }

    public bool IsDead => isDead;
}
