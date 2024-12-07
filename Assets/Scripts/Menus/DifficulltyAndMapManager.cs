using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficulltyAndMapManager : MonoBehaviour
{
    public static DifficulltyAndMapManager Instance { get; set; }

    public enum Difficulties
    {
        Easy,
        Middle,
        Hard
    }

    public Difficulties difficulty;
    public string map;

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
