using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThoseWhoKnowBonus : Bonus
{
    public Player player;
    public PlayerMovement playerMovement;

    protected override void DoItsThing()
    {
        BuffPlayer();
        StartCoroutine(WaitTillEnd());
        DeBuffPlayer();
        _isActive = false;
    }

    private void BuffPlayer()
    {
        player.maxHP *= 2;
        player.HP = player.maxHP;
        player.UpdateHP();

        playerMovement.speed *= 2;        
    }

    private void DeBuffPlayer()
    {
        player.maxHP /= 2;
        player.HP = player.maxHP;
        player.UpdateHP();

        playerMovement.speed /= 2;
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
    }
}