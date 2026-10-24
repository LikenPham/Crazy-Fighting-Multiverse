using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class AttackData
{
    public string attackName;
    public AnimKey animationKey = AnimKey.NormalAttack;
    [Min(0)] public int damage = 10;
    public Vector2 hitboxOffset = new Vector2(1f, 0f);
    public Vector2 hitboxSize = new Vector2(1f, 1f);
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
