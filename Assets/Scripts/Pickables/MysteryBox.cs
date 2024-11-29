using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MysteryBox : MonoBehaviour
{
    public Bonus.BonusTypes bonusType;

    void Start()
    {
        bonusType = Bonus.BonusTypes.Those;//(BonusTypes)Random.Range(0, System.Enum.GetValues(typeof(BonusTypes)).Length);        
    }

}
