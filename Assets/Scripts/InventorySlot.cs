using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

    public void AddItem(Bonus.BonusTypes bonusType, int count)
    {
        GameObject bonusInstance = Instantiate(BonusManager.Instance.bonusPrefab, bonusPanel);

        string name;
        string description;
        string imagePath;
        int duration;
        (name, description, imagePath, duration) = BonusesDB.Instance.GetBonusByName(bonusType.ToString());
        Bonus bonusScript = bonusInstance.GetComponent<Bonus>();

        switch (bonusType)
        {
            case Bonus.BonusTypes.Those:
            {
                bonusInstance.AddComponent<ThoseWhoKnowBonus>();
                bonusInstance.GetComponent<ThoseWhoKnowBonus>().Create(name, description, imagePath, duration);
                bonusInstance.GetComponent<ThoseWhoKnowBonus>().CloneBonus(bonusScript);
                bonusInstance.GetComponent<ThoseWhoKnowBonus>().pickedUp = true;
                Destroy(bonusScript);
                break;
            }
        }
        //UnityEditorInternal.ComponentUtility.MoveComponentUp(amount);
        amount.text = "x" + count.ToString();
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