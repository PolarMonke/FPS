using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MZombieChasingState : ZombieChasingState
{
    protected override void playSound()
    {
        Console.WriteLine("Minecraft");
        if (!SoundManager.Instance.mZombieChannel.isPlaying)
        {
            SoundManager.Instance.mZombieChannel.PlayOneShot(SoundManager.Instance.mZombieChasing);
        }
    }
    protected override void stopSound()
    {
        SoundManager.Instance.mZombieChannel.Stop();
    }
}
