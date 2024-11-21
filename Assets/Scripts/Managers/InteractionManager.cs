using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    private Weapon hoveredWeapon = null;
    private AmmoCrate hoveredAmmoCrate = null;

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
                hoveredWeapon = objectHitByRaycast.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
                
            }
            else if(hoveredWeapon == null) {}
            else
            {
                hoveredWeapon.GetComponent<Outline>().enabled = false;
            }

            if (objectHitByRaycast.GetComponent<AmmoCrate>())
            {
                hoveredAmmoCrate = objectHitByRaycast.GetComponent<AmmoCrate>();
                hoveredAmmoCrate.GetComponent<Outline>().enabled = true;
                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoCrate);
                    Destroy(objectHitByRaycast.gameObject);
                }
                
            }
            else if(hoveredAmmoCrate == null) {}
            else
            {
                hoveredAmmoCrate.GetComponent<Outline>().enabled = false;
            }
        }
    }
}
