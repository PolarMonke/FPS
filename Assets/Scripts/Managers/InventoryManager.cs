using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }

    private bool isPaused = false;
    public GameObject inventoryMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isPaused = !isPaused;
            PauseGame(isPaused);
        }
    }

    void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
        inventoryMenu.SetActive(pause);
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pause; 
    }
}
