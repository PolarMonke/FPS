using UnityEngine;

public class DummySpawn : MonoBehaviour
{
    public GameObject dummyPrefab;
    public GameObject gunPrefab;

    public Vector3 dummySpawnPosition;

    public void SpawnDummy()
    {
        dummySpawnPosition = transform.position;
        Instantiate(dummyPrefab, dummySpawnPosition, transform.rotation);    
    }
    public void RemoveDummy()
    {
        GameObject dummy = GameObject.FindGameObjectWithTag("Dummy");
        Destroy(dummy);
    }
    public void SpawnGun()
    {
        dummySpawnPosition = transform.position;
        Instantiate(gunPrefab, dummySpawnPosition, transform.rotation);
    }
    public void DestroyGun()
    {
        GameObject gun = GameObject.FindGameObjectWithTag("SpecialWeapon");
        gun.GetComponent<Weapon>().WeaponDespawn();
        Destroy(gun);
    }
}
