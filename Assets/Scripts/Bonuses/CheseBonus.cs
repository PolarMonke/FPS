using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class CheeseBonus : Bonus
{
    private DummySpawn dummySpawn;

    private void Start()
    {
        bonusType = BonusTypes.Cheese;
        dummySpawn = GameObject.FindGameObjectWithTag("DummySpawn").GetComponent<DummySpawn>();
    }

    protected override void DoItsThing()
    {
        _isActive = true;
        MoveToUI();
        AddGun();
        StartCoroutine(WaitTillEnd());
        _isActive = false;
    }

    private void AddGun()
    {
        dummySpawn.SpawnGun();
    }

    private void RemoveGun()
    {
        dummySpawn.DestroyGun();
        Deactivate();
    }

    private IEnumerator WaitTillEnd()
    {
        float timeLeft = _duration;

        while (timeLeft > 0)
        {
            Duration.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }
        RemoveGun();
    }
}