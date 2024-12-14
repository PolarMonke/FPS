using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    private IHoverable hoveredItem = null;
    private string currentHintText = "";


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
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;
            hoveredItem = GetHoverableItem(objectHitByRaycast);//works

            UpdateHintText(hoveredItem);

            if (Input.GetKeyDown(KeyCode.F) && hoveredItem != null)
            {
                HandleInteraction(hoveredItem, objectHitByRaycast);
            }
        }
        else
        {
            ClearHint();
        }
    }


    private IHoverable GetHoverableItem(GameObject obj)
    {
        if (obj.TryGetComponent<Weapon>(out var weapon) && !weapon.isActiveWeapon) return weapon as IHoverable;
        if (obj.TryGetComponent<AmmoCrate>(out var ammoCrate)) return ammoCrate as IHoverable;
        if (obj.TryGetComponent<HealthKit>(out var healthKit)) return healthKit as IHoverable;
        if (obj.TryGetComponent<MysteryBox>(out var mysteryBox)) return mysteryBox as IHoverable;
        if (obj.TryGetComponent<WeaponCrate>(out var weaponCrate)) return weaponCrate as IHoverable;
        return null;
    }




    private void UpdateHintText(IHoverable item)
    {
        if (item != null)
        {
            string newHint = "";
            if (item is Weapon weapon) newHint = weapon.weaponModel.ToString();
            else if (item is AmmoCrate ammoCrate) newHint = $"{LanguagesDB.Instance.GetText(ammoCrate.ammoType.ToString())}\n{ammoCrate.ammoAmount}";
            else if (item is HealthKit healthKit) newHint = $"{LanguagesDB.Instance.GetText("HealthKit")}\n{healthKit.healthAmount}";
            else if (item is MysteryBox mysteryBox) newHint = LanguagesDB.Instance.GetText("MysteryBox");
            else if (item is WeaponCrate weaponCrate) newHint = LanguagesDB.Instance.GetText("WeaponCrate");

            if (newHint != currentHintText)
            {
                currentHintText = newHint;
                HUDManager.Instance.DisplayHint(currentHintText);
            }
        }
        else
        {
            ClearHint();
        }
    }


    private void HandleInteraction(IHoverable item, GameObject obj)
    {
        if (item is Weapon weapon) WeaponManager.Instance.PickupWeapon(obj);
        else if (item is AmmoCrate ammoCrate)
        {
            WeaponManager.Instance.PickupAmmo(ammoCrate);
            Destroy(obj);
        }
        else if (item is HealthKit healthKit)
        {
            HealthManager.Instance.player.Heal(healthKit.healthAmount);
            Destroy(obj);
        }
        else if (item is MysteryBox mysteryBox)
        {
            BonusManager.Instance.AddToInventory(mysteryBox.bonusType);
            Destroy(obj);
        }
        else if (item is WeaponCrate weaponCrate)
        {
            string weaponToSpawn = weaponCrate.weaponModel.ToString();
            Vector3 spawnPos = obj.transform.position;
            spawnPos.y += 1;
            Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/Weapons/{weaponToSpawn}.prefab"), spawnPos, obj.transform.rotation);
            Destroy(obj);
        }

        ClearHint();
    }

    private void ClearHint()
    {
        currentHintText = "";
        HUDManager.Instance.UnDisplayHint();
        hoveredItem = null;
    }
}

public interface IHoverable { }
