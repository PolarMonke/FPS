using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;

    private float despawnTime = 30f;

    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
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
            StartCoroutine(DestroyZombieAfterTime(despawnTime));
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

    public IEnumerator DestroyZombieAfterTime(float delay)
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
