using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

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

    private void Start()
    {
        CharacterData character = CharacterPresetManager.Instance.characterData;

        GameObject weapon = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/Weapons/{character.WeaponModel}.prefab");
        //GameObject weapon = Resources.Load<GameObject>($"Prefabs/Weapons/{character.WeaponModel}"); 
        WeaponManager.Instance.PickupWeapon(Instantiate(weapon));

        Bonus.BonusTypes bonusType;
        Enum.TryParse(character.BonusType, true, out bonusType);
        BonusManager.Instance.AddToInventory(bonusType);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
