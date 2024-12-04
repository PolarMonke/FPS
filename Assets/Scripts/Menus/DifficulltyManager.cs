using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficulltyManager : MonoBehaviour
{
    public static DifficulltyManager Instance { get; set; }

    public enum Difficulties
    {
        Easy,
        Middle,
        Hard
    }

    public Difficulties difficulty;

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
