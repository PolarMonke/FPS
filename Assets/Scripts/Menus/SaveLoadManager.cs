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

    private string filePath = "Save.json";

    public GameObject loadingScreenPrefab;

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
            File.WriteAllText(filePath, json);
            Debug.Log("Game saved successfully to: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}");
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadGame());
    }
    public IEnumerator LoadGame()
    {
        SaveData loadData = new SaveData();
        try
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"File not found: {filePath}");
            }
            string json = File.ReadAllText(filePath);
            loadData = JsonConvert.DeserializeObject<SaveData>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading JSON file: {e.Message}");
        }
        
        Instantiate(loadingScreenPrefab);

        DifficulltyAndMapManager.Instance.map = loadData.map;
        Enum.TryParse(loadData.difficulty, true, out DifficulltyAndMapManager.Instance.difficulty);

        print(loadData.map);
        SceneManager.LoadScene(loadData.map);
        yield return new WaitForSeconds(1);

        int totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo;
        (totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo) = loadData.GetAmmo();
        WeaponManager.Instance.SetAmmo(totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo);

        WeaponManager.Instance.SetWeaponSlotsWeapons(loadData.weaponSlot1, loadData.weaponSlot2);

        InventoryManager.Instance.SetBonuses(loadData.bonuses);

        WaveManager.Instance.waveController.StartFromWave(loadData.wave);
    }

}
