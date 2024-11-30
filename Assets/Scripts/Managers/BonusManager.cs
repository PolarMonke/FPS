using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public bool dummySpawned = false;

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

    public void AddToInventory(Bonus.BonusTypes bonusType)
    {
        string name;
        string description;
        string imagePath;
        int duration;
        (name, description, imagePath, duration) = BonusesDB.Instance.GetBonusByName(bonusType.ToString());

        GameObject bonusInstance = Instantiate(bonusPrefab);
        bonusInstance.transform.SetParent(bonusCanvas.transform, false);
        Bonus bonusScript = bonusInstance.GetComponent<Bonus>();
        bonusScript.Create(name, description, imagePath, duration);
        bonusScript.pickedUp = true;
        bonusInstance.GetComponent<Animator>().SetTrigger("FadeOut");
        
        InventoryManager.Instance.AddToInventory(bonusType);
        StartCoroutine(DestroyBonus(bonusInstance));
    }

    private IEnumerator DestroyBonus(GameObject bonusInstance)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(bonusInstance);
    }


}

