using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject p1Frame;
    [SerializeField] private GameObject p2Frame;

    private int index;
    private Action<int> onHoverEnter;
    private Action onHoverExit;

    public void Setup(CharacterEntry entry, int index,
                      Action<int> onClick, Action<int> onHoverEnter, Action onHoverExit)
    {
        this.index = index;
        this.onHoverEnter = onHoverEnter;
        this.onHoverExit = onHoverExit;

        icon.sprite = entry.icon;
        p1Frame.SetActive(false);
        p2Frame.SetActive(false);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick(index));
    }

    public void SetMarked(int playerNumber, bool value)
    {
        (playerNumber == 1 ? p1Frame : p2Frame).SetActive(value);
    }

    public void OnPointerEnter(PointerEventData e) => onHoverEnter?.Invoke(index);
    public void OnPointerExit(PointerEventData e) => onHoverExit?.Invoke();
}