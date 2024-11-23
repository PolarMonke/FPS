using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MZombie : Enemy
{
    public ZombieHand zombieHand;
    public int zombieDamage = 20;
    void Start()
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
            animator.SetTrigger("Die");

            isDead = true;

            SoundManager.Instance.mZombieChannel.PlayOneShot(SoundManager.Instance.mZombieDying);

            StartCoroutine(DestroyEnemyAfterTime(despawnTime));
        }
        else
        {
            System.Random random = new System.Random();
            bool chooseAction1 = random.NextDouble() < 0.5; 

            if (chooseAction1)
            {
                SoundManager.Instance.mZombieChannel.PlayOneShot(SoundManager.Instance.mZombieHurt1);
            }
            else
            {
                SoundManager.Instance.mZombieChannel.PlayOneShot(SoundManager.Instance.mZombieHurt2);
            }
            animator.SetTrigger("Damage");
        }
    }


}
