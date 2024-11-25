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

            StartCoroutine(DestroyEnemyAfterTime(despawnTime));
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

    public virtual IEnumerator DestroyEnemyAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f);
    }
}
