using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MZombiePatrolingState : ZombiePatrolingState
{
    protected override void playSound()
    {
        if (!SoundManager.Instance.mZombieChannel.isPlaying)
        {
            
            System.Random random = new System.Random();
            double randomNumber = random.NextDouble();

            if (randomNumber < 0.8) 
            {
                
            }
            else if (randomNumber < 0.9)
            {
                SoundManager.Instance.mZombieChannel.PlayOneShot(SoundManager.Instance.mZombieWalking1);
            }
            else 
            {
                SoundManager.Instance.mZombieChannel.PlayOneShot(SoundManager.Instance.mZombieWalking2);
                
            }
        }
    }
    protected override void stopSound()
    {
        SoundManager.Instance.mZombieChannel.Stop();
    }
}
