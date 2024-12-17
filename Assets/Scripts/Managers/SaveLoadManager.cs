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

    private string saveFilePath = "Assets/Saves/Save.json";
    public string savesFilePath = "Assets/Saves/Saves.json";

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
        if (AccountManager.Instance.isLogged)
        {
            HighScoreDB.Instance.SaveHighScore(AccountManager.Instance.username, score);
        }
        else
        {
            PlayerPrefs.SetInt(highScoreKey, score);
        }
        
    }
    public int LoadHighScore()
    {
        if (AccountManager.Instance.isLogged)
        {
            string username = AccountManager.Instance.username;
            if (HighScoreDB.Instance.UserHasRecords(username))
            {
                return HighScoreDB.Instance.LoadUserScore(username);
            }
            else
            {
                return 0;
            }
        }
        else
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
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.map = DifficulltyAndMapManager.Instance.map;
        saveData.difficulty = DifficulltyAndMapManager.Instance.difficulty.ToString();

        saveData.playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().HP;

        int totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo;
        (totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo) = WeaponManager.Instance.GetAmmo();
        saveData.SetAmmo(totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo);

        (saveData.weaponSlot1, saveData.weaponSlot2) = WeaponManager.Instance.GetWeaponSlotsWeapons();

        saveData.bonuses = InventoryManager.Instance.GetAllBonuses();

        saveData.wave = WaveManager.Instance.waveController.currentWave;

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        try
        {
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Game saved successfully to: " + saveFilePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}");
        }

        if (AccountManager.Instance.isLogged)
        {
            SaveGameForUser(AccountManager.Instance.username, saveData);
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadGame());
    }
    public IEnumerator LoadGame()
    {
        if (AccountManager.Instance.isLogged)
        {
            LoadGameFromUser(AccountManager.Instance.username);
        }

        SaveData loadData = new SaveData();
        try
        {
            if (!File.Exists(saveFilePath))
            {
                Debug.LogError($"File not found: {saveFilePath}");
            }
            string json = File.ReadAllText(saveFilePath);
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
        print("Waited");

        int totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo;
        (totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo) = loadData.GetAmmo();
        WeaponManager.Instance.SetAmmo(totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo);

        WeaponManager.Instance.SetWeaponSlotsWeapons(loadData.weaponSlot1, loadData.weaponSlot2);

        InventoryManager.Instance.SetBonuses(loadData.bonuses);

        WaveManager.Instance.waveController.StartFromWave(loadData.wave);

        print("Game loaded");
    }
    //Saves user data to Saves.json
    public void SaveGameForUser(string username, SaveData saveData)
    {
        Dictionary<string, UserSaveData> userSaveData;

        if (File.Exists(savesFilePath))
        {
            string loadedJson = File.ReadAllText(savesFilePath);
            try
            {
                userSaveData = JsonConvert.DeserializeObject<Dictionary<string, UserSaveData>>(loadedJson);
            }
            catch (JsonReaderException)
            {
                Debug.LogError("Invalid JSON format in save file.  Creating a new file.");
                userSaveData = new Dictionary<string, UserSaveData>();
            }

        }
        else
        {
            userSaveData = new Dictionary<string, UserSaveData>();
        }

        if (userSaveData.ContainsKey(username))
        {
            userSaveData[username] = new UserSaveData(username, saveData);
        }
        else
        {
            userSaveData.Add(username, new UserSaveData(username, saveData));
        }

        string jsonToSave = JsonConvert.SerializeObject(userSaveData, Formatting.Indented);
        File.WriteAllText(savesFilePath, jsonToSave); 
    }
    //loads gamedata into Save.json from user
    public void LoadGameFromUser(string username)
    {
        if (!File.Exists(savesFilePath))
        {
            Debug.LogError($"Save file not found: {savesFilePath}");
            return;
        }

        string loadedJson = File.ReadAllText(savesFilePath);
        
        Dictionary<string, UserSaveData> loadedData = JsonConvert.DeserializeObject<Dictionary<string, UserSaveData>>(loadedJson);

        if (loadedData.ContainsKey(username))
        {
            UserSaveData userSave = loadedData[username];

            SaveData saveData = userSave.saveData;
            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(saveFilePath, json);
        }
        else
        {
            Debug.LogWarning($"No save data found for user: {username}");
        }
    }

    public void DeleteUserSaveData(string username)
    {
        if (!File.Exists(savesFilePath))
        {
            Debug.LogError($"Save file not found: {savesFilePath}");
            return;
        }

        string loadedJson = File.ReadAllText(savesFilePath);
        
        Dictionary<string, UserSaveData> loadedData = JsonConvert.DeserializeObject<Dictionary<string, UserSaveData>>(loadedJson);

        if (loadedData.ContainsKey(username))
        {
            loadedData.Remove(username);
            string jsonToSave = JsonConvert.SerializeObject(loadedData, Formatting.Indented);
            File.WriteAllText(savesFilePath, jsonToSave); 
        }
        else
        {
            Debug.LogWarning($"No save data found for user: {username}");
        }
    }
}
