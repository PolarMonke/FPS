using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }

    private bool isPaused = false;
    public GameObject inventoryMenu;

    private Dictionary<Bonus.BonusTypes, int> bonuses = new Dictionary<Bonus.BonusTypes, int>();
    
    public Transform inventoryPanel;
    public GameObject inventorySlotPrefab;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int numColumns = 4;
    public int numRows = 3; 

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isPaused = !isPaused;
            PauseGame(isPaused);
        }
    }

    void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
        inventoryMenu.SetActive(pause);
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pause; 
    }

    private void Start()
    {
        CreateInventoryGrid();
    }

    public Dictionary<string, int> GetAllBonuses()
    {
        Dictionary<string, int> bonusesToGet = new Dictionary<string, int>();

        foreach (KeyValuePair<Bonus.BonusTypes, int> bonus in bonuses)
        {
            string bonusType = bonus.Key.ToString();
            int bonusValue = bonus.Value;

            if (bonusesToGet.ContainsKey(bonusType))
            {
                Debug.LogError($"Duplicate bonus type found: {bonusType}. Overwriting value.");
            }
            bonusesToGet.Add(bonusType, bonusValue);
        }
        return bonusesToGet;
    }

    public void SetBonuses(Dictionary<string, int> bonusesToSet)
    {
        bonuses.Clear();
        foreach (KeyValuePair<string, int> kvp in bonusesToSet)
        {
            string bonusTypeString = kvp.Key;
            int bonusValue = kvp.Value;

            if (Enum.TryParse(bonusTypeString, true, out Bonus.BonusTypes bonusType))
            {
                if (bonuses.ContainsKey(bonusType))
                {
                    Debug.LogError($"Duplicate bonus type found: {bonusType}. Overwriting value.");
                }
                bonuses.Add(bonusType, bonusValue);
            }
            else
            {
                Debug.LogError($"Invalid bonus type: {bonusTypeString}. Skipping.");
            }
        }
    }
    
    private void CreateInventoryGrid()
    {
        RectTransform panelRect = inventoryPanel.GetComponent<RectTransform>();

        if (panelRect == null)
        {
            Debug.LogError("Inventory Panel does not have a RectTransform!");
            return;
        }

        float slotWidth = inventorySlotPrefab.GetComponent<RectTransform>().rect.width;
        float slotHeight = inventorySlotPrefab.GetComponent<RectTransform>().rect.height;


        for (int i = 0; i < numRows * numColumns; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
            slot.name = "Slot " + i;
            InventorySlot slotScript = slot.GetComponent<InventorySlot>();
            if (slotScript == null)
            {
                Debug.LogError("InventorySlot component missing from prefab!");
                continue; 
            }

            inventorySlots.Add(slotScript);
            slotScript.slotIndex = i;

            RectTransform rect = slot.GetComponent<RectTransform>();

            float x = 200 + i % numColumns * (slotWidth + 400f);
            float y = -200 + -(i / numColumns) * (slotHeight + 200f);

            rect.anchoredPosition = new Vector2(x, y);
        }
    }

    public void AddToInventory(Bonus.BonusTypes bonusType)
    {
        if (bonuses.ContainsKey(bonusType))
        {
            bonuses[bonusType] += 1;
        }
        else
        {
            bonuses.Add(bonusType, 1);
        }
        UpdateUI();
    }

    public void RemoveFromInventory(Bonus.BonusTypes bonus)
    {
        bonuses[bonus] -= 1;
        if (bonuses[bonus] <= 0)
        {
            bonuses.Remove(bonus);
        }
        UpdateUI();
        
    }

    private void UpdateUI()
    {
        foreach(InventorySlot slot in inventorySlots)
        {
            slot.ClearSlot();
        }

        foreach (KeyValuePair<Bonus.BonusTypes, int> kvp in bonuses)
        {
            print(kvp);
            int slotIndex = FindEmptySlot();
            if (slotIndex != -1)
            {
                inventorySlots[slotIndex].AddItem(kvp.Key,kvp.Value);
            }
            else
            {
                Debug.LogWarning("Inventory is full!");
                break; 
            }

        }
    }
    private int FindEmptySlot()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].isEmpty)
            {
                return i;
            }
        }
        return -1;
    }
}
