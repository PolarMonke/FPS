using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class DummySpawn : MonoBehaviour
{
    public GameObject dummyPrefab;

    public void SpawnDummy()
    {
        Instantiate(dummyPrefab);    
    }
    public void RemoveDummy()
    {
        Destroy(dummyPrefab);
    }
}
