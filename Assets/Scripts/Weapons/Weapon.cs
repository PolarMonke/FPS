using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour, IHoverable
{
    public enum WeaponModel
    {
        Colt1911,
        AK74,
        M4,
        M107,
        TvorahGun
    }
    public bool isActiveWeapon;
    public WeaponModel weaponModel;

    [Header("Sprites")]
    public Sprite weaponImage;
    public Sprite ammoImage;

    protected bool isShooting;
    protected bool readyToShoot;
    protected bool isReloading;

    [Header("Ammo")]
    //public int totalAmmo = 30;
    public int magCapacity = 10;
    public int ammoLeft = 10;
    public float reloadTime = 2;
    public int bulletDamage = 20;

    [Header("Fire rate")]
    public float fireRate = 0.7f;
    protected float nextFireTime;

    [Header("Spread intensity")]
    public float hipSpreadIntensity;
    public float inScopeSpreadIntensity;
    protected float spreadIntensity;
    
    [Header("Bullet options")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3;

    [Header("Animator")]
    public Animator animator;
    protected string shootAnimation = "Shooting";
    protected string reloadAnimation = "Reloading";
    protected string EnterScopeAnimation = "EnterScope";
    protected string ExitScopeAnimation = "ExitScope";
    protected string shootInScopeAnimation = "ShootingInScope";
    protected string reloadInScopeAnimation = "ReloadingInScope";
    
    [Header("Muzzle effect")]
    public GameObject muzzleEffect;
    protected bool inScope;

    [Header("Position")]
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    
    protected void Awake()
    {
        readyToShoot = true;
        spreadIntensity = hipSpreadIntensity;
    }

    protected virtual void Update()
    {
        if (isActiveWeapon)
        {
            WeaponSpawn();
            GoToScope();

            readyToShoot = ammoLeft > 0;
            ReloadIfNeeded();
            Shoot();
        }
        else
        {
            WeaponDespawn();
        }
    }
    protected void WeaponSpawn()
    {
        gameObject.layer = LayerMask.NameToLayer("WeaponRender");
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            foreach (Transform toddler in child)
            {
                toddler.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }
        }
    }
    public void WeaponDespawn()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform toddler in child)
            {
                toddler.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
        gameObject.GetComponent<Rigidbody>().isKinematic  = false;
    }

    protected virtual void Shoot()
    {
        isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        if (isShooting && ammoLeft == 0)
        {
            SoundManager.Instance.emptyMagSound.Play();
        }
        print("isShooting: " + isShooting);
        print("readyToShoot: " + readyToShoot);
        print("isReloading: " + isReloading);
        print("can fire: " + (Time.time >= nextFireTime));
        if (isShooting && readyToShoot && !isReloading && Time.time >= nextFireTime)
        {

            FireWeapon();
            if (isShooting)
            {
                nextFireTime = Time.time + 1f * fireRate;
            }
        }
    }

    protected virtual void ReloadIfNeeded()
    {
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
    }

    protected void GoToScope()
    {
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
    }

    

    protected virtual IEnumerator Reload()
    {
        isReloading = true;

        int magSpace = magCapacity - ammoLeft;

        int ammoToLoad = Mathf.Min(magSpace, WeaponManager.Instance.CheckAmmoLeft(weaponModel));

        yield return new WaitForSeconds(reloadTime);
        print("reload complete");

        ammoLeft += ammoToLoad;
        WeaponManager.Instance.DecreaseTotalAmmo(weaponModel, ammoToLoad);

        isReloading = false;
    }

    

    protected virtual void FireWeapon()
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

        readyToShoot = false;
        ammoLeft -= 1;

        Vector3 shootingDirecition = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirecition;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirecition * bulletVelocity, ForceMode.Impulse);
        bullet.GetComponent<Bullet>().SetDamage(bulletDamage);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
    
    }

    protected Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        
        return direction + new Vector3(x,y,0);
    }
    
    protected IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    } 

}
