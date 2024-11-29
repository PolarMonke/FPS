using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ChillGuyBonus : Bonus
{
    private DummySpawn dummySpawn;

    private void Start()
    {
        bonusType = BonusTypes.Invincible;
        dummySpawn = GameObject.FindGameObjectWithTag("DummySpawn").GetComponent<DummySpawn>();
    }

    protected override void DoItsThing()
    {
        MoveToUI();
        SpawnChillGuy();
        StartCoroutine(WaitTillEnd());
        _isActive = false;
    }

    private void SpawnChillGuy()
    {
        dummySpawn.SpawnDummy();
    }

    private void DeSpawnChillGuy()
    {
        dummySpawn.RemoveDummy();
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
        DeSpawnChillGuy();
    }
}