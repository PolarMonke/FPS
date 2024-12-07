using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; set; }

    public GameObject settingsMenu;
    public MainMenu mainMenu;

    private const string LANGUAGE_KEY = "MyString";
    private const string VOLUME_KEY = "MyFloat";

    public string language;
    public float volume;

    public TMP_Dropdown languagesDropdown;
    public Slider volumeSlider;

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
    private void Start()
    {
        LoadSettings();
    }

    public void EnterSettings()
    {
        settingsMenu.SetActive(true);
    }
    public void LoadSettings()
    {
        language = PlayerPrefs.GetString(LANGUAGE_KEY);
        volume = PlayerPrefs.GetFloat(VOLUME_KEY);
        ApplySettings();
    }
    private void SetSettings()
    {
        if (languagesDropdown.options[languagesDropdown.value].text.ToString() == "Беларуская")
        {
            language = "Belarusian";
        }
        else
        {
            language = "English";
        }
        
        volume = volumeSlider.value;

    }
    private void ApplySettings()
    {
        SoundManager.Instance.SetVolume(volume);
        LanguagesDB.Instance.SQL_TABLE_NAME = language;
        LanguagesDB.Instance.LoadLanguageData();
        mainMenu.UpdateLocalization();
    }
    public void SaveSettings()
    {
        SetSettings();
        PlayerPrefs.SetString(LANGUAGE_KEY, language);
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        ApplySettings();
    }
    public void ExitSettings()
    {
        SaveSettings();
        settingsMenu.SetActive(false);
    }
}
