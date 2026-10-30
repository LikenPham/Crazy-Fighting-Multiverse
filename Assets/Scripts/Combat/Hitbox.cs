using System;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private Transform hitboxOrigin;
    [SerializeField] private Transform damageSource;
    [SerializeField] private MovementModule movementModule;

    [SerializeField] private LayerMask targetLayers;
    [SerializeField, Min(1)] private int resultBufferSize = 16;

    [SerializeField] private AttackDataSO attackData;

    private Collider2D[] results;
    private ContactFilter2D filter;
    private readonly HashSet<IDamageable> alreadyHit = new();
    private AttackData selectedAttack;
    private AttackData currentAttack;

    public bool IsActive => currentAttack != null;

    private void Awake()
    {
        if (hitboxOrigin == null) hitboxOrigin = transform;
        if (damageSource == null) damageSource = transform;
        if (movementModule == null) movementModule = GetComponent<MovementModule>();

        results = new Collider2D[resultBufferSize];

        filter = new ContactFilter2D();
        filter.SetLayerMask(targetLayers);
        filter.useTriggers = true;
    }

    private void FixedUpdate()
    {
        if (currentAttack == null) return;
        Scan();
    }
    
    public void Begin(AttackData attack)
    {
        if (attack == null) return;
        currentAttack = attack;
        alreadyHit.Clear();
    }

    public void Activate(AnimKey key)
    {
        Begin(attackData.GetAttack(key));
    }

    public void Deactivate()
    {
        currentAttack = null;
        alreadyHit.Clear();
    }

    private Vector2 GetWorldCenter(AttackData attack)
    {
        Vector2 offset = attack.hitboxOffset;
        if (movementModule != null && !movementModule.FacingRight)
        {
            offset.x = -offset.x;
        }

        return (Vector2)hitboxOrigin.position + offset;
    }

    private void Scan()
    {
        Vector2 center = GetWorldCenter(currentAttack);
        int count = Physics2D.OverlapBox(center, currentAttack.hitboxSize, 0f, filter, results);

        for (int i = 0; i < count; i++)
        {
            Collider2D col = results[i];
            if (col == null) continue;

            if (col.transform.IsChildOf(damageSource)) continue;

            IDamageable receiver = col.GetComponent<IDamageable>();
            if (receiver == null) continue ;

            if (!alreadyHit.Add(receiver)) continue;

            receiver.TakeDamage(currentAttack.damage, damageSource);
        }
    }

    public void SelectAttack(AnimKey key) => selectedAttack = attackData.GetAttack(key);

    public void HitboxOn()
    {
        if (selectedAttack == null) return;
        currentAttack = selectedAttack;
        alreadyHit.Clear();
    }
    public void HitboxOff() => Deactivate();

    private void OnDisable()
    {
        Deactivate();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (attackData == null) return;

        Transform origin = hitboxOrigin != null ? hitboxOrigin : transform;
        bool facingRight = movementModule == null || movementModule.FacingRight;

        foreach (AttackData attack in attackData.AttackList)
        {
            bool isCurrent = attack == currentAttack;
            if (!attack.showGizmo && !isCurrent) continue;

            Vector2 offset = attack.hitboxOffset;
            if (!facingRight) offset.x = -offset.x;

            Gizmos.color = isCurrent ? Color.red : new Color(1f, 0.5f, 0f, 0.4f);

            Gizmos.DrawWireCube((Vector2)origin.position + offset, attack.hitboxSize);
        }
    }
#endif
}
