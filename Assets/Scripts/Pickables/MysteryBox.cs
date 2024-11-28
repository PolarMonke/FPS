using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BonusManager;

public class MysteryBox : MonoBehaviour
{
    public BonusTypes bonusType;

    void Start()
    {
        bonusType = BonusTypes.Those;//(BonusTypes)Random.Range(0, System.Enum.GetValues(typeof(BonusTypes)).Length);        
    }

}
