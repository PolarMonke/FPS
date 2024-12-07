using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPresetManager : MonoBehaviour
{
    public static CharacterPresetManager Instance { get; set; }

    public CharacterData characterData = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }
}