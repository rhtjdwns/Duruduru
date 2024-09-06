using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float TotalDamage { get; set; }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().IsInvincible) return;

            other.GetComponent<Player>().TakeDamage(TotalDamage);
        }
    }
}
