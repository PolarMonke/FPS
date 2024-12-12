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
    public CharactersListManager charactersListManager;

    public void StartGame()
    {
        if (charactersListManager.selectedCharacter != null)
        {
            charactersListManager.errorField.text = "";
            SceneManager.LoadScene(DifficulltyAndMapManager.Instance.map);
        }
        else
        {
            charactersListManager.errorField.text = "Select a character";
        }
        
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(newGameScene);
    }
}
