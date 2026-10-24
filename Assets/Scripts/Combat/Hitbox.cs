using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private Transform hitboxOrigin;
    [SerializeField] private Transform damageSource;
    [SerializeField] private LayerMask targetLayers;

    [SerializeField] private AttackDataSO attackData;
    [SerializeField] private bool facingRight = true;
}
