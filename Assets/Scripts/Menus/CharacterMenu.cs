using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Services.Analytics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CharacterMenu : MonoBehaviour
{
    private string newGameScene = "NewGameOptions";

    public void StartGame()
    {
        SceneManager.LoadScene(DifficulltyAndMapManager.Instance.map);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(newGameScene);
    }
}
