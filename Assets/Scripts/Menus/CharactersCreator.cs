using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Services.Analytics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreator : MonoBehaviour
{

    public TMP_InputField nameInputField;
    public TMP_Dropdown weaponsDropdown;
    public TMP_Dropdown bonusesDropdown;

    public TextMeshProUGUI namePlaceHolder;
    public TextMeshProUGUI addButton;
    public TextMeshProUGUI startButton;

    public CharactersListManager charactersListManager;

    private List<string> availableWeapons;
    private Dictionary<string, string> availableBonuses;

    private string selectedName;
    private string selectedWeapon;
    private string selectedBonus;

    private void Start()
    {
        AddWeapons();
        AddBonuses();
        namePlaceHolder.text = LanguagesDB.Instance.GetText("EnterName");
        addButton.text = LanguagesDB.Instance.GetText("Add");
        startButton.text = LanguagesDB.Instance.GetText("StartGame");
    }

    private void AddWeapons()
    {
        availableWeapons = WeaponsDB.Instance.GetAllWeaponModels();
        weaponsDropdown.AddOptions(availableWeapons);

    }
    
    private void AddBonuses()
    {
        availableBonuses = BonusesDB.Instance.GetAllBonusNames();
        bonusesDropdown.AddOptions(availableBonuses.Values.ToList());
    }  

    private void GetSelectedOptions()
    {
        selectedName = nameInputField.text;
        selectedWeapon = availableWeapons[weaponsDropdown.value];
        selectedBonus = availableBonuses.FirstOrDefault(x => x.Value == bonusesDropdown.options[bonusesDropdown.value].text).Key;
    }

    public void AddCharacter()
    {
        GetSelectedOptions();
        if (selectedName != "")
        {
            charactersListManager.errorField.text = "";
            if (AccountManager.Instance.isLogged)
            {
                charactersListManager.AddCharacter(selectedName, selectedWeapon, selectedBonus, AccountManager.Instance.username);
            }
            else
            {
                charactersListManager.AddCharacter(selectedName, selectedWeapon, selectedBonus);
            }
        }
        else
        {
            charactersListManager.errorField.text = "Pick a name";
        }
        
        
    }
}
