using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static SpeedManager Instance { get; set; }

    public PlayerMovement playerMovement;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
    }
}
