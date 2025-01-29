using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public int damage = 10;

    public bool destoryAfterHit;
    public bool disableColliderAfterHit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.Damage(damage);  // Apply damage to the enemy
            if (destoryAfterHit)
            {
                Destroy(gameObject);   // Destroy the bullet on collision
            }else if (disableColliderAfterHit)
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
