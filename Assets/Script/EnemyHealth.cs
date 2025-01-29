using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int MaxHealth = 100;
    [SerializeField] private int CurrentHealth;
    public int particleCount = 5;  // Number of particles to emit on destruction
    public int particleSize = 1;
    public float moveSpeed = 2f;   // Speed at which the enemy moves left
    [SerializeField] Rigidbody2D rb;

    void Start()
    {
        CurrentHealth = MaxHealth;  // Initialize health
    }

    public void Damage(int amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Vector3 spawnPosition = transform.position;  // Use enemy's position to spawn particles

            for (int i = 0; i < particleCount; i++)
            {
                particleSize +=(int) (Random.value * 3)-1;
                ParticleEmitter.SpawnParticle(spawnPosition, particleSize);  // Adjust size as needed
            }

            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        // Keep the enemy moving to the left with constant speed
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
    }

}
