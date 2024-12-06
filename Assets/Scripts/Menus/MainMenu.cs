using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;
    public TMP_Text NewGameText;
    public TMP_Text ContinueText;

    string newGameScene = "NewGameOptions";

    void Start()
    {
        UpdateLocalization();
    }

    public void UpdateLocalization()
    {
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"{LanguagesDB.Instance.GetText("TopWave")}{highScore}";
        NewGameText.text = LanguagesDB.Instance.GetText("NewGame");
        ContinueText.text = LanguagesDB.Instance.GetText("Continue");
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
