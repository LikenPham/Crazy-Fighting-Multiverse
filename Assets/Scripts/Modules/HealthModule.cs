using UnityEngine;

public class HealthModule : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private MovementModule movementModule;
    [SerializeField, Range(0f, 1f)] private float blockDamage = 0.1f;

    private int currentHealth;
    private bool isDead;

    public bool isBlocking { get; set;}
    public bool isInvinciblle {  get; set;}

    private void Awake()
    {
        currentHealth = maxHealth;
        if (movementModule == null) movementModule = GetComponent<MovementModule>();
    }

    public void TakeDamage(int damage, Transform source)
    {
        if (isDead || isInvinciblle) return;

        if (isBlocking && IsFrontal(source))
            damage = Mathf.RoundToInt(damage * blockDamage);

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

    private bool IsFrontal(Transform source)
    {
        if (source == null || movementModule == null) return false;

        float dx = source.position.x - transform.position.x;
        return dx >= 0f ? movementModule.FacingRight : !movementModule.FacingRight;
    }
}
