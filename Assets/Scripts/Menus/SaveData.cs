using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveData
{
    public string map;
    public string difficulty;

    public int playerHP;
    
    public string weaponSlot1;
    public string weaponSlot2;

    public int totalPistolAmmo;
    public int totalRifleAmmo;
    public int totalShotgunAmmo;
    public int totalSniperRifleAmmo;

    public Dictionary<string, int> bonuses;
    public int wave;

    public void SetAmmo(int totalPistolAmmo, int totalRifleAmmo, int totalShotgunAmmo, int totalSniperRifleAmmo)
    {
        this.totalPistolAmmo = totalPistolAmmo;
        this.totalRifleAmmo = totalRifleAmmo;
        this.totalShotgunAmmo = totalShotgunAmmo;
        this.totalSniperRifleAmmo = totalSniperRifleAmmo;
    }

    public (int, int, int, int) GetAmmo()
    {
        return (totalPistolAmmo, totalRifleAmmo, totalShotgunAmmo, totalSniperRifleAmmo);
    }
    
}

public class UserSaveData
{
    public string username;
    public SaveData saveData;

    public UserSaveData(string username, SaveData saveData)
    {
        this.username = username;
        this.saveData = saveData;
    }
}