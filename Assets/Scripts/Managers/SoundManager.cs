using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; set; }

    [Header("Player")]
    public AudioSource playerChannel;

    public AudioClip playerHurt;
    public AudioClip playerDead;

    public AudioClip catLaugh;

    [Header("Guns")]
    public AudioSource ShootingChannel;
    public AudioSource ReloadingChannel;

    public AudioSource emptyMagSound;

    public AudioClip Colt1911Shot;
    public AudioClip Colt1911ReloadingSound;

    public AudioClip AK74Shot;
    public AudioClip AK74ReloadingSound;

    public AudioClip ShotgunShot;
    public AudioClip ShotgunReloadingSound;
    public AudioClip ShotgunFinishReloading;

    public AudioClip SniperRifleShot;
    public AudioClip SniperRifleReloadingSound;

    public AudioClip TvorahShot;

    [Header("Zombie")]
    public AudioSource zombieChannel;

    public AudioClip zombieWalking;
    public AudioClip zombieChasing;
    public AudioClip zombieAttacking;
    public AudioClip zombieHurt;
    public AudioClip zombieDying;

    [Header("MZombie")]
    public AudioSource mZombieChannel;

    public AudioClip mZombieWalking1;
    public AudioClip mZombieWalking2;
    public AudioClip mZombieChasing;
    public AudioClip mZombieAttacking;
    public AudioClip mZombieHurt1;
    public AudioClip mZombieHurt2;
    public AudioClip mZombieDying;

    [Header("Skibidi")]
    public AudioSource skibidiChannel;
    public AudioClip skibidiMusic;

    
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public void SetVolume(float volume)
    {
        playerChannel.volume = volume;
        ShootingChannel.volume = volume;
        ReloadingChannel.volume = volume;
        zombieChannel.volume = volume;
        emptyMagSound.volume = volume;
        zombieChannel.volume = volume;
        mZombieChannel.volume = volume;
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Colt1911:
                ShootingChannel.PlayOneShot(Colt1911Shot);
                break;
            case WeaponModel.AK74:
                ShootingChannel.PlayOneShot(AK74Shot);
                break;
            case WeaponModel.M4:
                ShootingChannel.PlayOneShot(ShotgunShot);
                break;
            case WeaponModel.M107:
                ShootingChannel.PlayOneShot(SniperRifleShot);
                break;
            case WeaponModel.TvorahGun:
                ShootingChannel.PlayOneShot(TvorahShot);
                break;
        }
    }
    public void PlayReloadingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Colt1911:
                ReloadingChannel.PlayOneShot(Colt1911ReloadingSound);
                break;
            case WeaponModel.AK74:
                ReloadingChannel.PlayOneShot(AK74ReloadingSound);
                break;
            case WeaponModel.M4:
                ShootingChannel.PlayOneShot(ShotgunReloadingSound);
                break;
            case WeaponModel.M107:
                ShootingChannel.PlayOneShot(SniperRifleReloadingSound);
                break;
        }
    }
    public void PlayEndingReloadSound()
    {
        ShootingChannel.PlayOneShot(ShotgunFinishReloading);
    }

    
}
