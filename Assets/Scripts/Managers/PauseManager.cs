using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; set; }

    public TextMeshProUGUI mainMenuButtonText;

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
        mainMenuButtonText.text = LanguagesDB.Instance.GetText("ToMainMenu");
    }

    private bool isPaused = false;
    public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseGame(isPaused);
        }
    }

    void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
        pauseMenu.SetActive(pause);
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pause; 
    }

    public void ForceUnpauseGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}
