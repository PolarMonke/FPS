using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MZombieAttackingState : ZombieAttackingState
{
    protected override void playSound()
    {
        if (!SoundManager.Instance.mZombieChannel.isPlaying)
        {
            SoundManager.Instance.mZombieChannel.PlayOneShot(SoundManager.Instance.mZombieAttacking);
        }
    }
    protected override void stopSound()
    {
        SoundManager.Instance.mZombieChannel.Stop();
    }
}
