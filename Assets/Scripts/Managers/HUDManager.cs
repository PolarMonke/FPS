using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    
    public GameObject middleDot;

    public Image activeWeaponUI;
    public Image unactiveWeaponUI;

    public Image ammoTypeUI;
    public TextMeshProUGUI ammoUI;

    public Sprite transparentUI;

    public Image KeyHintUI;
    public TextMeshProUGUI KeyHintTextUI;

    public Sprite fKeyHint;

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

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unactiveWeapon = WeaponManager.Instance.GetUnactiveWeaponSlot().GetComponentInChildren<Weapon>();
        if (activeWeapon)
        {
            ammoUI.text = $"{activeWeapon.ammoLeft}/{WeaponManager.Instance.CheckAmmoLeft(activeWeapon.weaponModel)}";
            ammoTypeUI.sprite = activeWeapon.ammoImage;
            activeWeaponUI.sprite = activeWeapon.weaponImage;

            //FIXME: Weapons are scaling with image
            activeWeaponUI.transform.localScale = Vector3.one; //This did not fix it
            ammoTypeUI.transform.localScale = Vector3.one;


            if (unactiveWeapon)
            {
                unactiveWeaponUI.sprite = unactiveWeapon.weaponImage;
                unactiveWeaponUI.transform.localScale = new Vector3(0.5f,0.5f,1);
            }
        }
        else
        {
            ammoTypeUI.sprite = transparentUI;
            activeWeaponUI.sprite = transparentUI;
            unactiveWeaponUI.sprite = transparentUI;
            ammoUI.text = $"";
        }
    }  
    public void DisplayHint(string text)
    {
        KeyHintUI.gameObject.SetActive(true); 
        KeyHintTextUI.gameObject.SetActive(true);
        KeyHintUI.sprite = fKeyHint;
        KeyHintTextUI.text = text;
    }
    public void UnDisplayHint()
    {
        KeyHintUI.gameObject.SetActive(false);
        KeyHintTextUI.gameObject.SetActive(false);
    }
}
