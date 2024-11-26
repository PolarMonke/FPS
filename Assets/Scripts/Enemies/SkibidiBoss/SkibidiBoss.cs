using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkibidiBoss : Zombie
{
    public override void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            animator.SetTrigger("Die");

            isDead = true;

            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieDying);

            StartCoroutine(DestroyEnemyAfterTime(despawnTime));
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
}
