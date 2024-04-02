using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaCollision : MonoBehaviour
{
    public int damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tengu"))
        {
            TenguHealth enemyHealth = other.GetComponent<TenguHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
}
