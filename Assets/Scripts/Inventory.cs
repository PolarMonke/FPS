using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<Bonus, int> bonuses = new Dictionary<Bonus, int>();
    
    public Transform inventoryPanel;
    public GameObject inventorySlotPrefab;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int numColumns = 4;
    public int numRows = 3; 

    private void Start()
    {
        CreateInventoryGrid();
    }
    
    private void CreateInventoryGrid()
    {
        for (int i = 0; i < numRows * numColumns; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
            slot.name = "Slot " + i;
            InventorySlot slotScript = slot.GetComponent<InventorySlot>();

            inventorySlots.Add(slotScript);
            slotScript.slotIndex = i;
            

            RectTransform rect = slot.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2( i % numColumns * (rect.rect.width + 5f) , -(i / numColumns) * (rect.rect.height + 5f));
        }
    }

    public void AddToInventory(Bonus bonus)
    {
        if (bonuses.ContainsKey(bonus))
        {
            bonuses[bonus] += 1;
        }
        else
        {
            bonuses.Add(bonus, 1);
        }
        UpdateUI();
    }

    public void RemoveFromInventory(Bonus bonus)
    {
        if (bonuses.ContainsKey(bonus))
        {
            bonuses[bonus] -= 1;
            if (bonuses[bonus] <= 0)
            {
                bonuses.Remove(bonus);
            }
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        foreach(InventorySlot slot in inventorySlots)
        {
            slot.ClearSlot();
        }

        foreach (KeyValuePair<Bonus, int> kvp in bonuses)
        {
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
