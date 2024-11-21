using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _damage;

    internal void SetDamage(int bulletDamage)
    {
        _damage = bulletDamage;
    }

    private void OnCollisionEnter(Collision objectIHit)
    {
        if (objectIHit.gameObject.CompareTag("Target"))
        {
            print("hit" + objectIHit.gameObject.name + $"\nDealt {_damage} damage");
            
            Destroy(gameObject);
        }
        if (objectIHit.gameObject.CompareTag("Wall"))
        {
            print("hit a wall");
            //CreateBulletImpactEffect(objectIHit);
            Destroy(gameObject);
        }
    }
    
    void CreateBulletImpactEffect(Collision objectIHit)
    {
        ContactPoint contact = objectIHit.contacts[0];
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        hole.transform.SetParent(objectIHit.gameObject.transform);
    }
}
