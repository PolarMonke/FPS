using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Shotgun : Weapon
{
    public int bulletsPerShot = 9;

    protected string stopReloading = "ExitReloading";
    protected string stopReloadingInScope = "ExitReloadingInScope";


    protected override void Shoot()
    {
        isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        if (isShooting && ammoLeft == 0)
        {
            SoundManager.Instance.emptyMagSound.Play();
        }
        if (isShooting && readyToShoot && !isReloading && Time.time >= nextFireTime)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                FireWeapon();    
            }
            readyToShoot = false;
            ammoLeft -= 1;
            if (isShooting)
            {
                nextFireTime = Time.time + 1f * fireRate;
            }
        }
    }

    protected override void ReloadIfNeeded()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && WeaponManager.Instance.CheckAmmoLeft(weaponModel) > 0 && ammoLeft != magCapacity)
        {
            StartCoroutine(Reload());
            if (inScope)
            {
                animator.SetTrigger(reloadInScopeAnimation);
            }
            else
            {
                animator.SetTrigger(reloadAnimation);
            }
        }
    }
    
    protected override IEnumerator Reload()
    {
        isReloading = true;

        int magSpace = magCapacity - ammoLeft;

        int ammoToLoad = Mathf.Min(magSpace, WeaponManager.Instance.CheckAmmoLeft(weaponModel));

        SoundManager.Instance.PlayReloadingSound(weaponModel);
        
        yield return new WaitForSeconds(reloadTime);
        if (ammoToLoad > 0)
        {
            ammoLeft += 1;
        }
        WeaponManager.Instance.DecreaseTotalAmmo(weaponModel, 1);

        if (Input.GetKeyDown(KeyCode.Mouse0) || ammoToLoad == 0 || Input.GetKeyDown(KeyCode.Mouse1))
        {
            StopReloading();
        }
        else 
        {
            StartCoroutine(Reload());
        }
    }

    protected void StopReloading()
    {
        if (inScope)
            {
                animator.SetTrigger("ExitReloadingInScope");
            }
            else
            {
                animator.SetTrigger("ExitReloading");
            }
        isReloading = false;
    }

    protected override void FireWeapon()
    {
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        if (inScope)
        {
            animator.SetTrigger(shootInScopeAnimation);
        }
        else
        {
            animator.SetTrigger(shootAnimation);
        }
        SoundManager.Instance.PlayShootingSound(weaponModel);

        Vector3 shootingDirecition = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirecition;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirecition * bulletVelocity, ForceMode.Impulse);
        bullet.GetComponent<Bullet>().SetDamage(bulletDamage);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
    
    }

}