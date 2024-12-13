using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;
using System.Collections;

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
                ForceScale(bonusInstance, new Vector3(4f,4f,1f));
                bonusInstance.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
            }
        }
    }

    private void ForceScale(GameObject obj, Vector3 scale)
    {
        StartCoroutine(ForceScaleCoroutine(obj, scale));
    }

    private IEnumerator ForceScaleCoroutine(GameObject obj, Vector3 scale)
    {
        yield return new WaitForSeconds(1f);
        obj.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,0f);
        obj.GetComponent<RectTransform>().localScale = scale;
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

