using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class InvincibleBonus : Bonus
{
    private void Start()
    {
        bonusType = BonusTypes.Invincible;
    }

    protected override void DoItsThing()
    {
        MoveToUI();
        BuffPlayer();
        StartCoroutine(WaitTillEnd());
        _isActive = false;
    }

    private void BuffPlayer()
    {
        HealthManager.Instance.player.isInvincible = true;
    }

    private void DeBuffPlayer()
    {
        HealthManager.Instance.player.isInvincible = false;
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
        DeBuffPlayer();
    }
}