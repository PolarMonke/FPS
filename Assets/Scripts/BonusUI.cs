using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;

public class BonusUI : MonoBehaviour
{
    public float verticalSpacing = 200f;
    public float verticalPadding = 100f; 

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

    public void AlignChildrenVertically()
    {
        List<Bonus> bonuses = new List<Bonus>();

        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            Bonus bonus = gameObject.transform.GetChild(i).GetComponent<Bonus>();
            if (bonus != null)
            {
                bonuses.Add(bonus);
            }
        }

        RectTransform canvasRect = GetComponent<RectTransform>();
        float yPosition = canvasRect.rect.height - verticalPadding;
        float xPosition = verticalPadding;

        for (int i = 0; i < bonuses.Count; i++)
        {   
            RectTransform child = bonuses[i].GetComponent<RectTransform>();
            bonuses[i].GetComponent<RectTransform>().position = new Vector3(xPosition, yPosition - i * verticalSpacing, child.position.z);
        }
    }
}

