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
        switch (DifficulltyManager.Instance.difficulty)
        {
            case DifficulltyManager.Difficulties.Easy:
            {
                waveController.waveAdder = 5;
                break;
            }
            case DifficulltyManager.Difficulties.Middle:
            {
                waveController.waveAdder = 10;
                break;
            }
            case DifficulltyManager.Difficulties.Hard:
            {
                waveController.waveAdder = 15;
                break;
            }
        }
    }
}
