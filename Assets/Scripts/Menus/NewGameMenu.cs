using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameMenu : MonoBehaviour
{
    string mainMenuScene = "MainMenu";
    string Dust3Map = "Dust3";
    string TestMap = "TestMap";

    public ToggleGroup difficulties;
    public ToggleGroup maps;

    void Start()
    {
        //add localization
    }

    public void Submit()
    {
        Toggle selectedDifficulty = difficulties.ActiveToggles().FirstOrDefault();
        switch (selectedDifficulty.name)
        {
            case "Easy":
            {
                break;
            }
            case "Middle":
            {
                break;
            }
            case "Hard":
            {
                break;
            }
        }
        Toggle selectedMap = maps.ActiveToggles().FirstOrDefault();
        switch (selectedMap.name)
        {
            case "Dust3":
            {
                SceneManager.LoadScene(Dust3Map);
                break;
            }
            case "TestMap":
            {
                SceneManager.LoadScene(TestMap);
                break;
            }
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
