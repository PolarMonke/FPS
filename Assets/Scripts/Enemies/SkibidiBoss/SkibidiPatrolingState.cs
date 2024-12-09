using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkibidiPatrolingState : ZombiePatrolingState
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    protected override void playSound()
    {
        if (!SoundManager.Instance.skibidiChannel.isPlaying)
        {
            SoundManager.Instance.skibidiChannel.clip = SoundManager.Instance.skibidiMusic;
            SoundManager.Instance.skibidiChannel.PlayDelayed(1f);
        }
    }
    protected override void stopSound()
    {
        SoundManager.Instance.skibidiChannel.Stop();
    }
}
