using System.Collections;
using UnityEngine;

public class FlashModule : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] renderers;
    [SerializeField] private Color flashColor = new Color(1f, 0.3f, 0.3f);
    [SerializeField] private float flashInterval = 0.06f;
    [SerializeField] private int flashCount = 3;

    private Color[] originalColors;
    private Coroutine routine;

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<SpriteRenderer>();

        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].color;
    }

    public void Flash()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        for (int n = 0; n < flashCount; n++)
        {
            SetColor(flashColor);
            yield return new WaitForSeconds(flashInterval);
            Restore();
            yield return new WaitForSeconds(flashInterval);
        }
        routine = null;
    }

    private void SetColor(Color c)
    {
        foreach (var r in renderers) r.color = c;
    }

    private void Restore()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].color = originalColors[i];
    }

    private void OnDisable()
    {
        if (routine != null) { StopCoroutine(routine); routine = null; }
        if (originalColors != null) Restore();
    }
}