using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ThoseWhoKnowBonus : Bonus
{
    private void Start()
    {
        bonusType = BonusTypes.Those;
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
        HealthManager.Instance.player.maxHP *= 2;
        HealthManager.Instance.player.HP = HealthManager.Instance.player.maxHP;
        HealthManager.Instance.player.UpdateHP();

        SpeedManager.Instance.playerMovement.speed *= 3; 
        SpeedManager.Instance.playerMovement.jumpHeight *= 2;      
    }

    private void DeBuffPlayer()
    {
        HealthManager.Instance.player.maxHP /= 2;
        HealthManager.Instance.player.HP = HealthManager.Instance.player.maxHP;
        HealthManager.Instance.player.UpdateHP();

        SpeedManager.Instance.playerMovement.speed /= 3;
        SpeedManager.Instance.playerMovement.jumpHeight /= 2;  
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