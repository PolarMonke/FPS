using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Enemy
{

    public ZombieHand zombieHand;
    public int zombieDamage = 10;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        zombieHand.damage = zombieDamage;
    }
    public override void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            int randomValue = Random.Range(0,2);
            if (randomValue == 0)
            {
                animator.SetTrigger("Die1");
            }
            else
            {
                animator.SetTrigger("Die2");
            }

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
