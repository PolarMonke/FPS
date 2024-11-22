using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieHand zombieHandR;
    public ZombieHand zombieHandL;

    public int zombieDamage = 10;

    private void Start()
    {
        zombieHandL.damage = zombieDamage;
        zombieHandR.damage = zombieDamage;
    }

}
