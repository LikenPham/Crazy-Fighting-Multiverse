using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterEntry
{
    public string displayName;
    public Sprite icon;      // hình hiện trên UI
    public Sprite fullBody;
    public GameObject prefab;    // prefab nhân vật thật
}

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "DataConfig/Character Database")]
public class CharacterDatabaseSO : ScriptableObject
{
    [SerializeField] private List<CharacterEntry> characters = new();

    public int Count => characters.Count;

    // index chính là "số thứ tự" của nhân vật
    public CharacterEntry Get(int index)
    {
        if (index < 0 || index >= characters.Count)
        {
            Debug.LogWarning($"{name}: Character index {index} không tồn tại.", this);
            return null;
        }
        return characters[index];
    }
}