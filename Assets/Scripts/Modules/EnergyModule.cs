using System.Collections.Generic;
using UnityEngine;

public class EnergyModule : MonoBehaviour
{
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float regenPerSecond = 5f;

    private float energy;
    private readonly Dictionary<AnimKey, float> cooldownEnd = new();

    public float Energy => energy;
    public float Normalized => energy / maxEnergy;

    private void Awake()
    {
        energy = maxEnergy;
    }

    void Update()
    {
        energy = Mathf.Min(maxEnergy, energy + regenPerSecond * Time.deltaTime);
    }

    public bool CanUse( AnimKey key, AttackData data)
    {
        if (data == null) return false;
        if (energy < data.energyCost) return false;
        if (cooldownEnd.TryGetValue(key, out float end) && Time.time < end) return false;
        return true;
    }

    public void Consume(AnimKey key, AttackData data)
    {
        energy -= data.energyCost;
        if (data.cooldown > 0f)
            cooldownEnd[key] = Time.time + data.cooldown;
    }

    public float CooldownRemaining(AnimKey key)
    {
        return cooldownEnd.TryGetValue(key, out float end) ? Mathf.Max(0f, end - Time.time) : 0f;
    }
}
