using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalBulletMovement : MonoBehaviour
{

    public float speed = 5f;
    public float lifetime = 5f;
    public Vector2 WeaponVelocity;
    private Rigidbody2D rb;
    void Start()
    {
        if (speed!=0) {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D component missing on bullet");
                return;
            }

            Vector2 direction = transform.right;  // Move in the direction of the weapon's right
            rb.velocity = direction * speed+ WeaponVelocity;      // Apply velocity in the weapon's direction
        }
        Destroy(gameObject, lifetime);        // Destroy bullet after its lifetime
    }
}
