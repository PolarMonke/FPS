using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour, IHoverable
{
    private int minAmmoAmount = 10;
    private int maxAmmoAmount = 150;

    public int ammoAmount;
    public AmmoType ammoType;

    public enum AmmoType
    {
        RifleAmmo,
        PistolAmmo,
        ShotgunAmmo,
        SniperRifleAmmo
    }

    private void Start()
    {
        ammoType = (AmmoType)Random.Range(0, System.Enum.GetValues(typeof(AmmoType)).Length);

        ammoAmount = Random.Range(minAmmoAmount, maxAmmoAmount + 1);
    }
}
