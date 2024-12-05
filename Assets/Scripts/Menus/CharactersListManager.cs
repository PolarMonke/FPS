using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CharactersListManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public Transform characterListContent;

    private List<CharacterData> characters = new List<CharacterData>();
    private int lastID;

    public void AddCharacter(string name, string weaponModel, string bonusType)
    {
        CharacterData character = new CharacterData(lastID + 1, name, weaponModel, bonusType);
        characters.Add(character);
        lastID++;
        
        GameObject characterEntry = Instantiate(characterPrefab, characterListContent);
        characterEntry.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = character.Name;
        characterEntry.transform.Find("WeaponImage").GetComponent<Image>().sprite = WeaponsDB.Instance.GetSpriteByName(character.WeaponModel);
        characterEntry.transform.Find("BonusImage").GetComponent<Image>().sprite = BonusesDB.Instance.GetSpriteByName(character.BonusType);
    }



    public void PopulateCharacterList()
    {
        foreach (var character in characters)
        {
            GameObject characterEntry = Instantiate(characterPrefab, characterListContent);
            characterEntry.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = character.Name;
            characterEntry.transform.Find("WeaponImage").GetComponent<Image>().sprite = WeaponsDB.Instance.GetSpriteByName(character.WeaponModel);
            characterEntry.transform.Find("BonusImage").GetComponent<Image>().sprite = BonusesDB.Instance.GetSpriteByName(character.BonusType);
        }
    }
    
}
