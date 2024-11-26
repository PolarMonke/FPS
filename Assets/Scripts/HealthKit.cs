using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    public int healthAmount;

    private void Start()
    {

        healthAmount = Random.Range(20, 100);
    }
}
