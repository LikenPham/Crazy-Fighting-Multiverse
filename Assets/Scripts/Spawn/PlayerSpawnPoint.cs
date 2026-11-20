using UnityEngine;
using System;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CharacterDatabaseSO database;
    [SerializeField] private int playerNumber = 1;

    [Header("Player Setup")]
    [SerializeField] private string controlScheme = "Player1";
    [SerializeField] private string layerName = "Player1";
    [SerializeField] private LayerMask enemyLayers;   // layer của đối thủ
    [SerializeField] private bool faceRight = true;

    public static event Action<GameObject> PlayerSpawned;

    public GameObject Player { get; private set; }

    private void Start() => Spawn();

    public GameObject Spawn()
    {
        if (Player != null) return Player;

        int layer = LayerMask.NameToLayer(layerName);
        if (layer < 0)
        {
            Debug.LogError($"Layer '{layerName}' không tồn tại.", this);
            return null;
        }

        GameObject prefab = ResolvePrefab();
        if (prefab == null)
        {
            Debug.LogError("Không có prefab để spawn.", this);
            return null;
        }

        Player = Instantiate(prefab, transform.position, Quaternion.identity);
        Player.name = $"{prefab.name}_{controlScheme}";

        SetLayerRecursively(Player, layer);

        Player.GetComponent<InputReader>().SetControlScheme(controlScheme);

        Player.GetComponent<Hitbox>().SetTargetLayers(enemyLayers);
        Player.GetComponent<ProjectileLauncher>().SetTargetLayers(enemyLayers);

        Player.GetComponent<MovementModule>().SetFacing(faceRight);
        Player.GetComponent<MovementModule>().SetBlockLayers(enemyLayers);

        PlayerSpawned?.Invoke(Player);
        return Player;
    }

    private GameObject ResolvePrefab()
    {
        int index = MatchSelection.Get(playerNumber);

        if (database != null && index != MatchSelection.None)
        {
            CharacterEntry entry = database.Get(index);
            if (entry != null && entry.prefab != null) return entry.prefab;
        }

        return playerPrefab; // chưa chọn gì thì dùng prefab mặc định, tiện test
    }

    private static void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
            SetLayerRecursively(child.gameObject, layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = faceRight ? Color.green : Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawLine(transform.position,
            transform.position + (faceRight ? Vector3.right : Vector3.left));
    }
}