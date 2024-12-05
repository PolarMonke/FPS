using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterData
{
    public int ID;
    public string Name;
    public string WeaponModel;
    public string BonusType;

    public CharacterData(int id, string name, string weaponModel, string bonusType){
        ID = id;
        Name = name;
        WeaponModel = weaponModel;
        BonusType = bonusType;
    }
}