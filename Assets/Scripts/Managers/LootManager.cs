using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance { get; set; }

    public HealthKit healthKit;
    public AmmoCrate ammoCrate;
    public MysteryBox mysteryBox;
    public WeaponCrate weaponCrate;

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

    public void SpawnRandomLoot(Vector3 spawnPosition)
    {
        double randomNumber = Random.Range(0f, 1f);
        if (randomNumber < 0.1)
        {
            Instantiate(weaponCrate, spawnPosition, Quaternion.identity);
        }
        else if (randomNumber < 0.2)
        {
            Instantiate(mysteryBox, spawnPosition, Quaternion.identity);
        }
        else if (randomNumber < 0.3)
        {
            Instantiate(healthKit, spawnPosition, Quaternion.identity);
        }
        else
        {   
            Instantiate(ammoCrate, spawnPosition, Quaternion.identity);
        }
    }  

}
