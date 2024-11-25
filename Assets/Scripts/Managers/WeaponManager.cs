using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> WeaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    private int totalPistolAmmo = 100;
    private int totalRifleAmmo = 100;
    private int totalShotgunAmmo = 100;
    private int totalSniperRifleAmmo = 100;

    private void Awake()
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
    }

    private void Start()
    {
        activeWeaponSlot = WeaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in WeaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }
    }
    public void PickupWeapon(GameObject pickedupWeapon)
    {
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);
        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform,false);
        
        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();
        weapon.enabled = true;
        pickedupWeapon.GetComponent<Animator>().enabled = true;
        pickedupWeapon.GetComponent<Collider>().enabled = false;
        pickedupWeapon.GetComponent<Outline>().enabled = false;
;
        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);


        weapon.isActiveWeapon = true;
    }

    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().enabled = false;
            weaponToDrop.GetComponent<Animator>().enabled = false;
            weaponToDrop.GetComponent<Collider>().enabled = true;
            weaponToDrop.GetComponent<Outline>().enabled = true;


            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;

        }
    }

    private void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }
        activeWeaponSlot  = WeaponSlots[slotNumber];
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
            newWeapon.GetComponent<Outline>().enabled = false;
        }

    }
    public GameObject GetUnactiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponSlots)
        {
            if (weaponSlot != activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    } 

    internal void PickupAmmo(AmmoCrate ammoCrate)
    {
        switch (ammoCrate.ammoType)
        {
            case AmmoCrate.AmmoType.PistolAmmo:
                totalPistolAmmo += ammoCrate.ammoAmount; break;
            case AmmoCrate.AmmoType.RifleAmmo:
                totalRifleAmmo += ammoCrate.ammoAmount; break;
            case AmmoCrate.AmmoType.ShotgunAmmo:
                totalShotgunAmmo += ammoCrate.ammoAmount; break;
            case AmmoCrate.AmmoType.SniperRifleAmmo:
                totalSniperRifleAmmo += ammoCrate.ammoAmount; break;
        }
    }
    public int CheckAmmoLeft(WeaponModel weaponModel)
    {
        string model = weaponModel.ToString();
        string ammoType = WeaponsDB.Instance.GetAmmoTypeByWeapon(model);
        switch (ammoType)
        {
            case "PistolAmmo":
                return WeaponManager.Instance.totalPistolAmmo;
            case "RifleAmmo":
                return WeaponManager.Instance.totalRifleAmmo;
            case "ShotgunAmmo":
                return WeaponManager.Instance.totalShotgunAmmo;
            case "SniperRifleAmmo":
                return WeaponManager.Instance.totalSniperRifleAmmo;
            default:
                return 0;
        }
    }
    internal void DecreaseTotalAmmo(WeaponModel weaponModel, int ammoToLoad)
    {
        string model = weaponModel.ToString();
        string ammoType = WeaponsDB.Instance.GetAmmoTypeByWeapon(model);
        switch (ammoType)
        {
            case "PistolAmmo":
                totalPistolAmmo -= ammoToLoad; break;
            case "RifleAmmo":
                totalRifleAmmo -= ammoToLoad; break;
            case "ShotgunAmmo":
                totalShotgunAmmo -= ammoToLoad; break;
            case "SniperRifleAmmo":
                totalShotgunAmmo -= ammoToLoad; break;
        }
    }
}