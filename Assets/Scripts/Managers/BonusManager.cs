using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance { get; set; }

    [Header("Bonus Prefab")]
    public GameObject bonusPrefab; 
    public Canvas bonusCanvas; 

    [Header("Bonus Settings")]
    public float bonusSpacing = 50f;
    public float leftMargin = 50f;

    private List<Bonus> activeBonuses = new List<Bonus>();
    private Dictionary<string, Bonus> bonusByName = new Dictionary<string, Bonus>();

    public enum BonusTypes
    {
        Double,
        Invincible,
        Chill,
        Those,
    }

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
    }

}

