using System.Collections;
using System.Collections.Generic;
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

    public bool SpawnBonus(BonusData bonusData)
    {
        if (bonusByName.ContainsKey(bonusData.Name))
        {
            Debug.LogWarning($"Bonus with name '{bonusData.Name}' already exists!");
            return false;
        }

        GameObject bonusGO = Instantiate(bonusPrefab, bonusCanvas.transform);
        Bonus bonus = bonusGO.GetComponent<Bonus>();

        if (bonus == null)
        {
            Debug.LogError("Bonus prefab doesn't contain a Bonus script!");
            Destroy(bonusGO);
            return false;
        }


        bonus.Initialize(bonusData);
        bonus.gameObject.SetActive(true);

        RectTransform rectTransform = bonusGO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(leftMargin, -bonusSpacing * activeBonuses.Count);


        activeBonuses.Add(bonus);
        bonusByName.Add(bonusData.Name, bonus);
        return true;
    }

    public void DespawnBonus(string bonusName)
    {
        if (bonusByName.TryGetValue(bonusName, out Bonus bonus))
        {
            activeBonuses.Remove(bonus);
            bonusByName.Remove(bonusName);
            Destroy(bonus.gameObject);

            for (int i = 0; i < activeBonuses.Count; i++)
            {
                RectTransform rt = activeBonuses[i].GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(leftMargin, -bonusSpacing * i);
            }

        }
        else
        {
            Debug.LogWarning($"Bonus with name '{bonusName}' not found!");
        }
    }

    [System.Serializable]
    public class BonusData
    {
        public string Name;
        public string Description;
        public Sprite BGImage;
        public string Duration;
    }
}

