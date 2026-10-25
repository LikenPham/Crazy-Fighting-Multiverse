using UnityEngine;

public class HealthModule : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;

    private int currentHealth;
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Transform source)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
    }
}
