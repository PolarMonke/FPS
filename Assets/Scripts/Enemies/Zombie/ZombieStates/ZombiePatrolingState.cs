using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{
    protected float timer;
    public float patrolingTime = 10f;

    protected Transform player;
    protected NavMeshAgent agent;

    public float detectionAreaRadius = 20f;
    public float patrolSpeed = 2f;

    protected List<Transform> waypointsList = new List<Transform>();

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
        playSound();
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
        stopSound();
    }

    protected virtual void playSound()
    {
        if (!SoundManager.Instance.zombieChannel.isPlaying)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }
    }
    protected virtual void stopSound()
    {
        SoundManager.Instance.zombieChannel.Stop();
    }
}
