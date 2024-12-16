using UnityEngine;
using System.Collections.Generic;

public class BonusUI : MonoBehaviour
{
    public List<GameObject> bonusSlots = new List<GameObject>();

    public void PutBonusIntoSlot(GameObject bonusInstance)
    {
        for (int i = 0; i < bonusSlots.Count; i++)
        {
            bool isEmpty = bonusSlots[i].transform.childCount == 0;
            if (isEmpty)
            {
                bonusInstance.transform.SetParent(bonusSlots[i].transform);
                ForceScale(bonusInstance, new Vector3(4f, 4f, 1f));
                bonusInstance.GetComponent<RectTransform>().localPosition = Vector3.zero;
                break;
            }
        }
    }

    private void ForceScale(GameObject obj, Vector3 scale)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localScale = scale;
        }
        else
        {
            Debug.LogError("GameObject " + obj.name + " does not have a RectTransform!");
        }
    }

    public bool AllSlotsAreFull()
    {
        for (int i = 0; i < bonusSlots.Count; i++)
        {
            if (bonusSlots[i].transform.childCount == 0)
            {
                return false;
            }
        }
        return true;
    }
}

