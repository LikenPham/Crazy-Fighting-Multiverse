using System;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private Transform hitboxOrigin;
    [SerializeField] private Transform damageSource;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private AttackDataSO attackData;

    private readonly HashSet<Collider2D> alreadyHit = new();
    private AttackData currentAttack;

    public bool IsActive => currentAttack != null;
    public event Action<Collider2D, AttackData> OnHit;
}
