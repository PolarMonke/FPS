using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapon
{

    private bool isSpraying = false;

    protected override void Update()
    {
        if (isActiveWeapon)
        {
            gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetTrigger(EnterScopeAnimation);
                spreadIntensity = inScopeSpreadIntensity;
                inScope = true;
                HUDManager.Instance.middleDot.SetActive(false);
            }
            if (Input.GetMouseButtonUp(1))
            {
                animator.SetTrigger(ExitScopeAnimation);
                spreadIntensity = hipSpreadIntensity;
                inScope = false;
                HUDManager.Instance.middleDot.SetActive(true);
            }

            readyToShoot = ammoLeft > 0;

            if (Input.GetKeyDown(KeyCode.R) && !isReloading && WeaponManager.Instance.CheckAmmoLeft(weaponModel) > 0 && ammoLeft != magCapacity)
            {   
                StartCoroutine(Reload());
                SoundManager.Instance.PlayReloadingSound(weaponModel);
                if (inScope)
                {
                    animator.SetTrigger(reloadInScopeAnimation);
                }
                else
                {
                    animator.SetTrigger(reloadAnimation);
                }
                
            } 
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
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
        
        
    }


}