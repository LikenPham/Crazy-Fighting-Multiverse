using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CameraTargetRegistrar : MonoBehaviour
{
    [SerializeField] private float weight = 1f;
    [SerializeField] private float radius = 2f;

    private CinemachineTargetGroup targetGroup;

    private void Awake() => targetGroup = GetComponent<CinemachineTargetGroup>();

    private void OnEnable() => PlayerSpawnPoint.PlayerSpawned += AddTarget;
    private void OnDisable() => PlayerSpawnPoint.PlayerSpawned -= AddTarget;

    private void AddTarget(GameObject player)
    {
        targetGroup.AddMember(player.transform, weight, radius);
    }
}