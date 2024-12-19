using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;


public class CharactersListManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public Transform characterListContent;

    private List<CharacterData> characters = new List<CharacterData>();
    private int lastID;

    private Dictionary<int, GameObject> characterEntries = new Dictionary<int, GameObject>();
    public CharacterData selectedCharacter;
    private Image selectionHighlight;
    private Color selectedColor = Color.black;
    private Color defaultColor = Color.gray;

    public TextMeshProUGUI errorField;

    private void Start()
    {
        if (AccountManager.Instance.isLogged)
        {
            PopulateCharacterList(AccountManager.Instance.username);
        }
        else
        {
            PopulateCharacterList();
        }
    }

    public void AddCharacter(string name, string weaponModel, string bonusType)
    {
        lastID = ChractersDB.Instance.GetLastID();
        CharacterData character = new CharacterData(lastID + 1, name, weaponModel, bonusType, null);
        characters.Add(character);
        ChractersDB.Instance.SaveCharacter(character);
        
        InstantiateCharacter(character);
    }
    public void AddCharacter(string name, string weaponModel, string bonusType, string owner)
    {
        lastID = ChractersDB.Instance.GetLastID();
        CharacterData character = new CharacterData(lastID + 1, name, weaponModel, bonusType, owner);
        characters.Add(character);
        ChractersDB.Instance.SaveCharacter(character);
        
        InstantiateCharacter(character);
    }

    public void PopulateCharacterList()
    {
        characters = ChractersDB.Instance.LoadCharacters();
        if (characters.Count != 0)
        {
            foreach (var character in characters)
            {
                InstantiateCharacter(character);
            }
        }
    }
    public void PopulateCharacterList(string username)
    {
        characters = ChractersDB.Instance.LoadCharacters(username);
        if (characters.Count != 0)
        {
            foreach (var character in characters)
            {
                InstantiateCharacter(character);
            }
        }
    }


    private void InstantiateCharacter(CharacterData character)
    {
        GameObject characterEntry = Instantiate(characterPrefab, characterListContent);
        characterEntry.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = character.Name;
        characterEntry.transform.Find("WeaponImage").GetComponent<Image>().sprite = AssetsReferences.Instance.GetWeaponSprite(character.WeaponModel);
        characterEntry.transform.Find("BonusImage").GetComponent<Image>().sprite = BonusesDB.Instance.GetSpriteByName(character.BonusType);
        
        Transform deleteButtonContainer = characterEntry.transform.Find("DeleteButton");
        Button deleteButton = characterEntry.transform.Find("DeleteButton").GetComponent<Button>();
        if (deleteButton != null)
        {
            int charID = character.ID;
            deleteButton.onClick.AddListener(() => DeleteCharacter(charID));//FIXME: no onClick
        }

        Button selectButton = characterEntry.GetComponent<Button>();
        if(selectButton != null)
        {
            int charID = character.ID;
            selectButton.onClick.AddListener(() => SelectCharacter(charID));
        }
        
        selectionHighlight = characterEntry.GetComponent<Image>();
        if (selectionHighlight != null)
        {
            selectionHighlight.color = defaultColor;
        }

        characterEntries.Add(character.ID, characterEntry);
    }

    private void DeleteCharacter(int id)
    {
        ChractersDB.Instance.DeleteCharacter(id);

        foreach (var kvp in characterEntries)
        {
            Destroy(kvp.Value);
        }
        characterEntries.Clear();
        characters.Clear();
        selectedCharacter = null;

        lastID = 0;

        if (AccountManager.Instance.isLogged)
        {
            PopulateCharacterList(AccountManager.Instance.username);
        }
        else
        {
            PopulateCharacterList();
        }
    }

    private void SelectCharacter(int id)
    {
        if (selectedCharacter != null)
        {
            if (characterEntries.TryGetValue(selectedCharacter.ID, out GameObject prevSelectedEntry))
            {
                Image prevHighlight = prevSelectedEntry.GetComponent<Image>();
                if (prevHighlight != null)
                {
                    prevHighlight.color = defaultColor;
                }
            }
        }

        selectedCharacter = characters.Find(c => c.ID == id);
        if (selectedCharacter != null)
        {
            if (characterEntries.TryGetValue(selectedCharacter.ID, out GameObject selectedEntry))
            {
                selectionHighlight = selectedEntry.transform.GetComponent<Image>();
                if (selectionHighlight != null)
                {
                    selectionHighlight.color = selectedColor;
                }
                CharacterPresetManager.Instance.characterData = characters[id-1];
                print(characters[id-1].Name);
            }
            else
            {
                print(characters[id-1].Name);
                print(characters[id].Name);
                print(characters[id+1].Name);
                Debug.LogError($"Could not find GameObject for character ID: {selectedCharacter.ID}");
            }

        }
    }
}
