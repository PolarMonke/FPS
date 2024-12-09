using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    
    public TextMeshProUGUI title;
    public TextMeshProUGUI language;
    public TextMeshProUGUI sound;
    public TextMeshProUGUI exit;

    void Start()
    {
        title.text = LanguagesDB.Instance.GetText("Settings");
        language.text = LanguagesDB.Instance.GetText("Language");
        sound.text = LanguagesDB.Instance.GetText("Sound");
        exit.text = LanguagesDB.Instance.GetText("SaveAndExit");
    }
}
