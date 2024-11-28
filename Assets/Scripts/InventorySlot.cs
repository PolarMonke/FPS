using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public int slotIndex;
    public Bonus bonus;
    public TextMeshProUGUI amount;
    public Transform bonusPanel;
    public bool isEmpty = true;

    public void AddItem(Bonus bonusToAdd, int count)
    {
        bonus = bonusToAdd;
        GameObject.Instantiate(bonus, bonusPanel);
        amount.text = count.ToString();
        gameObject.SetActive(true);
        isEmpty = false;
    }

    public void ClearSlot()
    {
        Destroy(bonus);
        amount.text = "";
        gameObject.SetActive(false);
        isEmpty = true;
    }
}
