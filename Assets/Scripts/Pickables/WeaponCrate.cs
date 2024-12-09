using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCrate : MonoBehaviour
{
    public Weapon.WeaponModel weaponModel;

    void Start()
    {
        weaponModel = (Weapon.WeaponModel)Random.Range(0, System.Enum.GetValues(typeof(Weapon.WeaponModel)).Length);        
    }
}
