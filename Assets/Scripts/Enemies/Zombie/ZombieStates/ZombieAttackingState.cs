using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackingState : StateMachineBehaviour
{
    protected Transform player;
    protected NavMeshAgent agent;

    public float stopAttackingDistance = 2.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LookAtPlayer();
        playSound();
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    protected void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0,yRotation,0);

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        stopSound();
    }

    protected virtual void playSound()
    {
        if (!SoundManager.Instance.zombieChannel.isPlaying)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieAttacking);
        }
    }
    protected virtual void stopSound()
    {
        SoundManager.Instance.zombieChannel.Stop();
    }
}
