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
    public string Owner;

    public CharacterData(int id, string name, string weaponModel, string bonusType, string owner){
        ID = id;
        Name = name;
        WeaponModel = weaponModel;
        BonusType = bonusType;
        Owner = owner;
    }
}