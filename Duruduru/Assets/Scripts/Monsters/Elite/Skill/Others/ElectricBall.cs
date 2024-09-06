using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : MonoBehaviour
{
    public float TotalDamage { get; set; } = 0;
    public float Speed { get; set; } = 0;
    public float Direction { get; set; } = 0;


    private void Update()
    {
        transform.Translate(new Vector2(Direction, 0) * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().IsInvincible) return;

            other.GetComponent<Player>().TakeDamage(TotalDamage);

            GameObject explosion = ObjectPool.Instance.Spawn("ElectricBallExplosion");
            explosion.transform.position = transform.position;
            ObjectPool.Instance.Remove(gameObject);
        }
    }
}
