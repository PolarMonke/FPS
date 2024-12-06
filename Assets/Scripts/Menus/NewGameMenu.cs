using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameMenu : MonoBehaviour
{
    string mainMenuScene = "MainMenu";
    string characterMenuScene = "CharacterMenu";
    string Dust3Map = "Dust3";
    string TestMap = "TestMap";

    public ToggleGroup difficulties;
    public ToggleGroup maps;

    public Text EasyText;
    public Text MiddleText;
    public Text HardText;
    public Text Dust3Text;
    public Text TestMapText;
    public TMP_Text SubmitText;

    void Start()
    {
        EasyText.text = LanguagesDB.Instance.GetText("Easy");
        MiddleText.text = LanguagesDB.Instance.GetText("Middle");
        HardText.text = LanguagesDB.Instance.GetText("Hard");
        Dust3Text.text = LanguagesDB.Instance.GetText("Dust3");
        TestMapText.text = LanguagesDB.Instance.GetText("TestMap");
        SubmitText.text = LanguagesDB.Instance.GetText("Submit");
    }

    public void Submit()
    {
        Toggle selectedDifficulty = difficulties.ActiveToggles().FirstOrDefault();
        switch (selectedDifficulty.name)
        {
            case "Easy":
            {
                DifficulltyAndMapManager.Instance.difficulty = DifficulltyAndMapManager.Difficulties.Easy;
                break;
            }
            case "Middle":
            {
                DifficulltyAndMapManager.Instance.difficulty = DifficulltyAndMapManager.Difficulties.Middle;
                break;
            }
            case "Hard":
            {
                DifficulltyAndMapManager.Instance.difficulty = DifficulltyAndMapManager.Difficulties.Hard;
                break;
            }
        }
        Toggle selectedMap = maps.ActiveToggles().FirstOrDefault();
        switch (selectedMap.name)
        {
            case "Dust3":
            {
                DifficulltyAndMapManager.Instance.map = Dust3Map;
                break;
            }
            case "TestMap":
            {
                DifficulltyAndMapManager.Instance.map = TestMap;
                break;
            }
        }
        SceneManager.LoadScene(characterMenuScene);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
