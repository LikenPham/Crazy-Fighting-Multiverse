using System.Collections.Generic;
using System;
using UnityEngine;

public enum AttackType { Melee, Ranged }
public enum AttackContext { Ground, Air, Both }

[Serializable]
public class AttackData
{
    public string attackName;
    public AnimKey animationKey = AnimKey.NormalAttack;
    public AttackType type = AttackType.Melee;
    [Min(0)] public int damage = 10;

    [Header("Melee")]
    public Vector2 hitboxOffset = new Vector2(1f, 0f);
    public Vector2 hitboxSize = new Vector2(1f, 1f);
    public bool showGizmo = true;

    [Header("Ranged")]
    public Projectile projectilePrefab;
    public Vector2 projectileSpawnOffset = new Vector2(1f, 0f);
    public float projectileSpeed = 12f;
    public float projectileLifetime = 2f;

    [Header("Cost")]
    [Min(0)] public float energyCost = 0f;
    [Min(0)] public float cooldown = 0f;

    [Header("Condition")]
    public AttackContext context = AttackContext.Ground;
}

[CreateAssetMenu(fileName = "AttackData", menuName = "DataConfig/Attack")]
public class AttackDataSO : ScriptableObject
{
    [SerializeField] private List<AttackData> attackList = new();

    public int Count => attackList.Count;

    public AttackData GetAttack(int index)
    {
        if (index < 0 || index >= attackList.Count)
        {
            Debug.LogWarning($"{name}: Attack index {index} không tồn tại.", this);
            return null;
        }
        return attackList[index];
    }

    public AttackData GetAttack(AnimKey key)
    {
        foreach (AttackData attack in attackList)
            if (attack.animationKey == key) return attack;

        Debug.LogWarning($"{name}: Không có attack nào với key {key}.", this);
        return null;
    }

#if UNITY_EDITOR
    public IReadOnlyList<AttackData> AttackList => attackList; // dùng cho Gizmos
#endif
}
