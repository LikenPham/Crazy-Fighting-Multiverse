using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private AttackDataSO attackData;
    [SerializeField] private MovementModule movement;
    [SerializeField] private Transform owner;          // root player
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private Transform firePoint;

    private AttackData selected;
    private readonly Dictionary<Projectile, ObjectPool<Projectile>> pools = new();

    private void Awake()
    {
        if (owner == null) owner = transform;
        if (movement == null) movement = GetComponent<MovementModule>();
    }

    public void SelectAttack(AnimKey key) => selected = attackData.GetAttack(key);

    // Animation Event gọi
    public void Fire()
    {
        if (selected == null || selected.projectilePrefab == null) return;

        bool right = movement.FacingRight;
        Vector2 pos = firePoint.position;

        ObjectPool<Projectile> pool = GetPool(selected.projectilePrefab);
        Projectile p = pool.Get();
        p.Launch(pos, right, selected, targetLayers, owner, proj => pool.Release(proj));
    }

    private ObjectPool<Projectile> GetPool(Projectile prefab)
    {
        if (pools.TryGetValue(prefab, out var pool)) return pool;

        pool = new ObjectPool<Projectile>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: p => p.gameObject.SetActive(true),
            actionOnRelease: p => p.gameObject.SetActive(false),
            actionOnDestroy: p => Destroy(p.gameObject),
            defaultCapacity: 8,
            maxSize: 32);

        pools[prefab] = pool;
        return pool;
    }
}