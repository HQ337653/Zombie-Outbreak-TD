using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subbulletTwoMovement : MonoBehaviour
{
    [SerializeField] public float initialXspeed;  // Initial horizontal speed
    [SerializeField] private float gravity = 15f;      // Gravitational acceleration
    [SerializeField] private GameObject endPrefab;      // Prefab to spawn when bullet reaches ground

    private Vector3 velocity;

    private void Start()
    {
        gravity += Random.value * 7;
        velocity = new Vector3(initialXspeed, 0, 0);  // Initial horizontal velocity
    }

    private void Update()
    {
        ApplyGravity();
        MoveAndAlignWithRotation();

        if (transform.position.y <= 0)
        {
            SpawnEndObject();
            Destroy(gameObject);  // Destroy bullet after reaching ground
        }
    }

    private void ApplyGravity()
    {
        velocity.y -= gravity * Time.deltaTime;  // Update vertical speed due to gravity
    }

    private void MoveAndAlignWithRotation()
    {
        transform.position += velocity * Time.deltaTime;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);  // Align rotation to velocity direction
    }

    private void SpawnEndObject()
    {
        if (endPrefab != null)
        {
           var g= Instantiate(endPrefab, transform.position, Quaternion.identity);
            g.SetActive(true);
        }
    }
}