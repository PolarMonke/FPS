using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsReferences : MonoBehaviour
{
    public static AssetsReferences Instance { get; set; }

    private Dictionary<string, Sprite> bonuses;
    private Dictionary<string, GameObject> weapons;
    private Dictionary<string, Sprite> weaponSprites;

    public List<Sprite> BonusesBackGrounds;
    public List<GameObject> WeaponPrefabs;
    public List<Sprite> WeaponSprites;
    
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

        bonuses = new Dictionary<string, Sprite>();
        weapons = new Dictionary<string, GameObject>();
        weaponSprites = new Dictionary<string, Sprite>();


        if (bonuses != null)
        {
            foreach (Sprite sprite in BonusesBackGrounds)
            {
                if (sprite != null)
                {
                    string spriteName = sprite.name;
                    if (!bonuses.ContainsKey(spriteName))
                    {
                         bonuses.Add(spriteName, sprite);
                         print(spriteName);
                    }
                    else
                    {
                        Debug.LogError($"Duplicate sprite name found: {spriteName}");
                    }
                }
                else
                {
                    Debug.LogError("Null sprite found in BonusesBackgroundSprites list.");
                }
            }
        }
        else
        {
            Debug.LogWarning("BonusesBackgroundSprites list is null.");
        }

        if (WeaponPrefabs != null)
        {
            foreach (GameObject prefab in WeaponPrefabs)
            {
                if (prefab != null)
                {
                    string prefabName = prefab.name;
                    if (!weapons.ContainsKey(prefabName))
                    {
                        weapons.Add(prefabName, prefab);
                    }
                    else
                    {
                        Debug.LogError($"Duplicate weapon prefab name found: {prefabName}");
                    }
                }
                else
                {
                    Debug.LogError("Null prefab found in WeaponPrefabs list.");
                }
            }
        }
        else
        {
            Debug.LogWarning("WeaponPrefabs list is null.");
        }

        if (WeaponSprites != null)
        {
            foreach (Sprite sprite in WeaponSprites)
            {
                if (sprite != null)
                {
                    string spriteName = sprite.name;
                    if (!weaponSprites.ContainsKey(spriteName))
                    {
                        weaponSprites.Add(spriteName, sprite);
                    }
                    else
                    {
                        Debug.LogError($"Duplicate weapon sprite name found: {spriteName}");
                    }
                }
                else
                {
                    Debug.LogError("Null sprite found in WeaponSpritesList list.");
                }
            }
        }
        else
        {
            Debug.LogWarning("WeaponSpritesList list is null.");
        }
    }

    public Sprite GetBonusBackground(string key)
    {
        if (bonuses.ContainsKey(key))
        {
            return bonuses[key];
        }
        else
        {
            Debug.LogError($"Bonus background with key '{key}' not found.");
            return null;
        }
    }

    public GameObject GetWeapon(string key)
    {
        if (weapons.ContainsKey(key))
        {
            return weapons[key];
        }
        else
        {
            Debug.LogError($"Weapon with key '{key}' not found.");
            return null;
        }
    }

    public Sprite GetWeaponSprite(string key)
    {
        if (weaponSprites.ContainsKey(key))
        {
            return weaponSprites[key];
        }
        else
        {
            Debug.LogError($"Weapon sprite with key '{key}' not found.");
            return null;
        }
    }
}
