using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{
    private float timer;
    public float patrolingTime = 10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionAreaRadius = 20f;
    public float patrolSpeed = 2f;

    private List<Transform> waypointsList = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;
        timer = 0;

        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in waypointCluster.transform)
        {
            waypointsList.Add(t);
        }

        Vector3 nextPosition = waypointsList[Random.Range(0,waypointsList.Count)].position;
        agent.SetDestination(nextPosition);

    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 nextPosition = waypointsList[Random.Range(0,waypointsList.Count)].position;
            agent.SetDestination(nextPosition);
        }

        timer += Time.deltaTime;
        if (timer > patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}
