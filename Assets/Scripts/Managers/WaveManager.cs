using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; set; }

    public ZombieSpawnController waveController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
        switch (DifficulltyAndMapManager.Instance.difficulty)
        {
            case DifficulltyAndMapManager.Difficulties.Easy:
            {
                waveController.waveAdder = 5;
                break;
            }
            case DifficulltyAndMapManager.Difficulties.Middle:
            {
                waveController.waveAdder = 10;
                break;
            }
            case DifficulltyAndMapManager.Difficulties.Hard:
            {
                waveController.waveAdder = 15;
                break;
            }
        }
    }
}
