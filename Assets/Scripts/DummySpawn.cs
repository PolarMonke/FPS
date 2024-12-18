using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DummySpawn : MonoBehaviour
{
    public GameObject dummyPrefab;
    public GameObject gunPrefab;

    private List<GameObject> guns = new List<GameObject>();
    private List<GameObject> dummies = new List<GameObject>();

    public Vector3 dummySpawnPosition;

    public void SpawnDummy()
    {
        dummySpawnPosition = transform.position;
        GameObject newDummy = Instantiate(dummyPrefab, dummySpawnPosition, transform.rotation);
        dummies.Add(newDummy);
    }

    public void RemoveDummy()
    {
        if (dummies.Count > 0)
        {
            GameObject dummy = dummies[0];
            if (dummy != null)
            {
                Destroy(dummy);
                dummies.RemoveAt(0);
            }
        }
    }
    public void SpawnGun()
    {
        dummySpawnPosition = transform.position;
        GameObject newGun = Instantiate(gunPrefab, dummySpawnPosition, transform.rotation);
        guns.Add(newGun);
    }
    public void DestroyGun()
    {
        if (guns.Count > 0)
        {
            GameObject gun = guns[0];
            if (gun != null && gun.GetComponent<Weapon>() != null)
            {
                gun.GetComponent<Weapon>().WeaponDespawn();
                Destroy(gun);
                guns.RemoveAt(0);
            }
        }

    }
}
