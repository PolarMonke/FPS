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

    public void AddToInventory(BonusTypes bonusName)
    {
        //Bonus bonusToAdd = BonusesDB.Instance.GetBonusByName(bonusName.ToString());
        string name;
        string description;
        string imagePath;
        int duration;
        (name, description, imagePath, duration) = BonusesDB.Instance.GetBonusByName(bonusName.ToString());

        GameObject bonusInstance = Instantiate(bonusPrefab);
        bonusInstance.transform.SetParent(bonusCanvas.transform, false);
        Bonus bonusScript = bonusInstance.GetComponent<Bonus>();
        switch (bonusName)
        {
            case BonusTypes.Those:
            {
                bonusInstance.AddComponent<ThoseWhoKnowBonus>();
                bonusInstance.GetComponent<ThoseWhoKnowBonus>().Create(name, description, imagePath, duration);
                bonusInstance.GetComponent<ThoseWhoKnowBonus>().CloneBonus(bonusScript);
                bonusInstance.GetComponent<ThoseWhoKnowBonus>().pickedUp = true;
                break;
            }
            default:
            {
                throw new NotImplementedException();
            }
        }
        bonusScript.enabled = false;
        bonusInstance.GetComponent<Animator>().SetTrigger("FadeOut");
    }

}

