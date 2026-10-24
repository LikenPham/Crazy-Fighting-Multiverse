using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AnimationMapping
{
    public AnimKey animKey;
    public string animName;
}

[CreateAssetMenu(fileName = "AnimData", menuName ="DataConfig")]
public class AnimationDataSO : ScriptableObject
{
    [SerializeField] private List<AnimationMapping> animationList;

    private readonly Dictionary<AnimKey, int> animHashDictionary = new();

    public void InitializeHashes()
    {
        animHashDictionary.Clear();

        foreach (AnimationMapping mapping in animationList)
        {
            if (mapping.animKey == AnimKey.None)
                continue;

            if (string.IsNullOrEmpty(mapping.animName))
                continue;

            int hash = Animator.StringToHash(mapping.animName);
            animHashDictionary[mapping.animKey] = hash;
        }
    }

    public int GetHash(AnimKey key)
    {
        if (animHashDictionary.TryGetValue(key, out int hash))
        {
            return hash;
        }

        Debug.LogWarning($"{name}: Missing animation hash for key {key}.", this);
        return 0;
    }
}
