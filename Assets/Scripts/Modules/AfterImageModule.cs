using System.Collections.Generic;
using UnityEngine;

public class AfterImageModule : MonoBehaviour
{
    [SerializeField] private SpriteRenderer source;
    [SerializeField] private float spawnInterval = 0.03f; // kho?ng cách gi?a các bóng
    [SerializeField] private float fadeTime = 0.25f;
    [SerializeField, Range(0f, 1f)] private float startAlpha = 0.6f;
    [SerializeField] private Color tint = new Color(0.5f, 0.8f, 1f);

    private class Ghost
    {
        public GameObject go;
        public SpriteRenderer sr;
        public float timer;
    }

    private readonly List<Ghost> active = new();
    private readonly Queue<Ghost> pool = new();

    private bool tracing;
    private float spawnTimer;

    private void Awake()
    {
        if (source == null) source = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (tracing)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnGhost();
                spawnTimer = spawnInterval;
            }
        }

        for (int i =active.Count - 1; i >= 0; i--)
        {
            Ghost ghost = active[i];
            ghost.timer -= Time.deltaTime;

            if (ghost.timer <= 0f)
            {
                ghost.go.SetActive(false);
                pool.Enqueue(ghost);
                active.RemoveAt(i);
                continue;
            }

            Color color = tint;
            color.a = startAlpha * (ghost.timer / fadeTime);
            ghost.sr.color = color;
        }
    }

    public void StartTrail()
    {
        tracing = true;
        spawnTimer = 0f;
    }

    public void StopTrail()
    {
        tracing = false;
    }

    private Ghost CreateGhost()
    {
        GameObject ghostObject = new GameObject("AfterImage");
        SpriteRenderer ghostRenderer = ghostObject.AddComponent<SpriteRenderer>();
        ghostRenderer.sharedMaterial = source.sharedMaterial;
        Ghost newGhost = new Ghost();
        newGhost.go = ghostObject;
        newGhost.sr = ghostRenderer;
        return newGhost;
    }

    private void SpawnGhost()
    {
        Ghost ghost = pool.Count > 0 ? pool.Dequeue() : CreateGhost();

        Transform trans = source.transform;
        ghost.go.transform.SetPositionAndRotation(transform.position, transform.rotation);
        ghost.go.transform.localScale = trans.lossyScale;

        ghost.sr.sprite =source.sprite;
        ghost.sr.flipX = source.flipX;
        ghost.sr.flipY = source.flipY;
        ghost.sr.sortingLayerID = source.sortingLayerID;
        ghost.sr.sortingOrder = source.sortingOrder;

        ghost.timer = fadeTime;
        ghost.go.SetActive(true);
        active.Add(ghost);
    }
}
