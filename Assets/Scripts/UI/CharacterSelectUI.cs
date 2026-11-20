using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private CharacterDatabaseSO database;
    [SerializeField] private CharacterSlotUI slotPrefab;
    [SerializeField] private Transform grid;

    [Header("Ảnh nguyên hình")]
    [SerializeField] private Image p1Art;
    [SerializeField] private Image p2Art;

    [Header("UI khác")]
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private string gameSceneName = "Game";

    private readonly List<CharacterSlotUI> slots = new();
    private int picker = 1; // đang đến lượt ai (1 hoặc 2), 0 = đã chọn xong

    private void Start()
    {
        for (int i = 0; i < database.Count; i++)
        {
            CharacterSlotUI slot = Instantiate(slotPrefab, grid);
            slot.Setup(database.Get(i), i, OnClickSlot, OnHoverSlot, OnHoverExit);
            slots.Add(slot);
        }

        startButton.onClick.AddListener(StartMatch);
        resetButton.onClick.AddListener(ResetSelection);
        ResetSelection();
    }

    private void ResetSelection()
    {
        MatchSelection.Reset();
        picker = 1;

        foreach (CharacterSlotUI s in slots)
        {
            s.SetMarked(1, false);
            s.SetMarked(2, false);
        }

        RefreshArt(1);
        RefreshArt(2);
        RefreshUI();
    }

    private void OnHoverSlot(int index)
    {
        if (picker == 0) return;           // chọn xong rồi thì không xem trước nữa
        ShowArt(picker, index);
    }

    private void OnHoverExit()
    {
        if (picker == 0) return;
        RefreshArt(picker);                // quay về nhân vật đã chốt (hoặc ẩn nếu chưa chọn)
    }

    private void OnClickSlot(int index)
    {
        if (picker == 0) return;

        MatchSelection.Set(picker, index);

        for (int i = 0; i < slots.Count; i++)
            slots[i].SetMarked(picker, i == index);

        ShowArt(picker, index);

        picker = picker == 1 ? 2 : 0;      // P1 xong -> P2 -> xong
        RefreshUI();
    }

    private void RefreshArt(int playerNumber)
    {
        int index = MatchSelection.Get(playerNumber);
        if (index == MatchSelection.None)
        {
            (playerNumber == 1 ? p1Art : p2Art).enabled = false;
            return;
        }
        ShowArt(playerNumber, index);
    }

    private void ShowArt(int playerNumber, int index)
    {
        Image art = playerNumber == 1 ? p1Art : p2Art;
        CharacterEntry entry = database.Get(index);
        if (entry == null) return;

        art.sprite = entry.fullBody;
        art.enabled = true;
    }

    private void RefreshUI()
    {
        turnText.text = picker switch
        {
            1 => "P1 chọn nhân vật",
            2 => "P2 chọn nhân vật",
            _ => "Sẵn sàng!"
        };
        startButton.interactable = MatchSelection.Ready;
    }

    private void StartMatch()
    {
        if (!MatchSelection.Ready) return;
        SceneManager.LoadScene(gameSceneName);
    }
}