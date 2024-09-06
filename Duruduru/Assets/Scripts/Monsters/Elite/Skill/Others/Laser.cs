using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float TotalDamage { get; set; }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().IsInvincible) return;

            other.GetComponent<Player>().TakeDamage(TotalDamage);
        }
    }
}
