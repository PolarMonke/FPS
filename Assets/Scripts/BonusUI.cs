using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;

public class BonusUI : MonoBehaviour
{
    public List<GameObject> bonusSlots = new List<GameObject>();

    public void PutBonusIntoSlot(GameObject bonusInstance)
    {
        for(int i = 0; i < bonusSlots.Count; i++)
        {
            bool isEmpty = bonusSlots[i].transform.childCount == 0;
            if(isEmpty)
            {
                bonusInstance.transform.SetParent(bonusSlots[i].transform);
                bonusInstance.GetComponent<RectTransform>().localScale = new Vector3(2f,2f,1f);
            }
        }
    }
    
    public bool AllSlotsAreFull()
    {
        for(int i = 0; i < bonusSlots.Count; i++)
        {
            bool isEmpty = bonusSlots[i].transform.childCount == 0;
            if(isEmpty)
            {
                return false;
            }
        }
        return true;
    }
}

