using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class CheseBonus : Bonus
{

    private void Start()
    {
        bonusType = BonusTypes.Invincible;
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
        HealthManager.Instance.player.isInvincible = true;
    }

    private void RemoveGun()
    {
        HealthManager.Instance.player.isInvincible = false;
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