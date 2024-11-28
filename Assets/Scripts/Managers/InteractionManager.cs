using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    private Weapon hoveredWeapon = null;
    private AmmoCrate hoveredAmmoCrate = null;
    private HealthKit hoveredHealthKit = null;
    private MysteryBox hoveredMysteryBox = null;

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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;
            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {

                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }

                hoveredWeapon = objectHitByRaycast.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;
                HUDManager.Instance.DisplayHint(hoveredWeapon.weaponModel.ToString());
                if (Input.GetKeyDown(KeyCode.F))
                {
                    HUDManager.Instance.UnDisplayHint();
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
                
            }
            else if(hoveredWeapon == null) {}
            else
            {
                hoveredWeapon.GetComponent<Outline>().enabled = false;
                HUDManager.Instance.UnDisplayHint();
            }

            if (objectHitByRaycast.GetComponent<AmmoCrate>())
            {

                if (hoveredAmmoCrate)
                {
                    hoveredAmmoCrate.GetComponent<Outline>().enabled = false;
                }

                hoveredAmmoCrate = objectHitByRaycast.GetComponent<AmmoCrate>();
                hoveredAmmoCrate.GetComponent<Outline>().enabled = true;
                HUDManager.Instance.DisplayHint(hoveredAmmoCrate.ammoType.ToString() + "\n" + hoveredAmmoCrate.ammoAmount.ToString());
                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoCrate);
                    HUDManager.Instance.UnDisplayHint();
                    Destroy(objectHitByRaycast.gameObject);
                }
                
            }
            else if(hoveredAmmoCrate == null) {}
            else
            {
                hoveredAmmoCrate.GetComponent<Outline>().enabled = false;
                HUDManager.Instance.UnDisplayHint();
            }

            if (objectHitByRaycast.GetComponent<HealthKit>())
            {

                if (hoveredHealthKit)
                {
                    hoveredHealthKit.GetComponent<Outline>().enabled = false;
                }

                hoveredHealthKit = objectHitByRaycast.GetComponent<HealthKit>();
                hoveredHealthKit.GetComponent<Outline>().enabled = true;
                HUDManager.Instance.DisplayHint("HealthKit" + "\n" + hoveredHealthKit.healthAmount.ToString());
                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    HealthManager.Instance.player.Heal(hoveredHealthKit.healthAmount);
                    HUDManager.Instance.UnDisplayHint();
                    Destroy(objectHitByRaycast.gameObject);
                }
                
            }
            else if(hoveredHealthKit == null) {}
            else
            {
                hoveredHealthKit.GetComponent<Outline>().enabled = false;
                HUDManager.Instance.UnDisplayHint();
            }
            
            if (objectHitByRaycast.GetComponent<MysteryBox>())
            {

                if (hoveredMysteryBox)
                {
                    hoveredMysteryBox.GetComponent<Outline>().enabled = false;
                }

                hoveredMysteryBox = objectHitByRaycast.GetComponent<MysteryBox>();
                hoveredMysteryBox.GetComponent<Outline>().enabled = true;
                HUDManager.Instance.DisplayHint("Mystery box");                     //localize
                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    BonusManager.Instance.AddToInventory(hoveredMysteryBox.bonusType);
                    HUDManager.Instance.UnDisplayHint();
                    Destroy(objectHitByRaycast.gameObject);
                }
                
            }
            else if(hoveredMysteryBox == null) {}
            else
            {
                hoveredMysteryBox.GetComponent<Outline>().enabled = false;
                HUDManager.Instance.UnDisplayHint();
            }
        }
    }
}
