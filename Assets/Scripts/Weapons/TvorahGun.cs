using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvorahGun : Weapon
{

    private bool isSpraying = false;

    protected override void Update()
    {
        if (isActiveWeapon)
        {
            WeaponSpawn();
            GoToScope();

            readyToShoot = ammoLeft > 0;
            if (!readyToShoot)
            {
                WeaponDespawn();
            }
            Shoot();
        }
        else
        {
            WeaponDespawn();
        }
    }

    protected override void Shoot()
    {
        isSpraying = Input.GetKey(KeyCode.Mouse0);
        isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        if (isShooting && ammoLeft == 0)
        {
            SoundManager.Instance.emptyMagSound.Play();
        }

        if (isSpraying && Time.time >= nextFireTime && readyToShoot && !isReloading)
        {
            FireWeapon();
            if (isSpraying) 
            {
                nextFireTime = Time.time + 1f * fireRate; 
            }
        }
        if (isShooting && readyToShoot && !isReloading && Time.time >= nextFireTime)
        {
            FireWeapon();
            if (isShooting) 
            {
                nextFireTime = Time.time + 1f * fireRate; 
            }
        }
    }
}