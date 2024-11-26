using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int HP = 100;
    protected Animator animator;

    protected NavMeshAgent navAgent;
    
    public bool isDead = false;

    public float despawnTime = 30f;

    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }
    public virtual void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            animator.SetTrigger("Die");

            isDead = true;

            StartCoroutine(DestroyEnemyAfterTime(despawnTime));
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

    public virtual IEnumerator DestroyEnemyAfterTime(float delay)
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 0.3f;
        LootManager.Instance.SpawnRandomLoot(spawnPosition);
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    
}
