using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }

    private string highScoreKey = "BestWaveSavedValue";

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

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(highScoreKey, score);
    }
    public int LoadHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey)) 
        {
            return PlayerPrefs.GetInt(highScoreKey);
        }
        else
        {
            return 0;
        }
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.map = DifficulltyAndMapManager.Instance.map;
        saveData.difficulty = DifficulltyAndMapManager.Instance.difficulty.ToString();

        saveData.playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().HP;
        //saveData.playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        //saveData.playerRotation = GameObject.FindGameObjectWithTag("Player").transform.rotation;

        int totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo;
        (totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo) = WeaponManager.Instance.GetAmmo();
        saveData.SetAmmo(totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo);

        (saveData.weaponSlot1, saveData.weaponSlot2) = WeaponManager.Instance.GetWeaponSlotsWeapons();

        saveData.bonuses = InventoryManager.Instance.GetAllBonuses();

        saveData.wave = WaveManager.Instance.waveController.currentWave;

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        try
        {
            File.WriteAllText("Save.json", json);
            Debug.Log("Game saved successfully to: " + "Save.json");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}");
        }
    }

    public void LoadGame()
    {

    }

}
