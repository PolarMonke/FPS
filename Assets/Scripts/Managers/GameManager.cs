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
        if (CharacterPresetManager.Instance.characterData != null)
        {
            CharacterData character = CharacterPresetManager.Instance.characterData;
            GameObject weapon = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/Weapons/{character.WeaponModel}.prefab");
            WeaponManager.Instance.PickupWeapon(Instantiate(weapon));

            Bonus.BonusTypes bonusType;
            Enum.TryParse(character.BonusType, true, out bonusType);
            BonusManager.Instance.AddToInventory(bonusType);
            print("Game started");
        }
    }

    public void ExitToMainMenu()
    {
        SaveLoadManager.Instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
}
