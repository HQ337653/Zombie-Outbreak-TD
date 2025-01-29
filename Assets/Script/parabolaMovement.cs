using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parabolaMovement : MonoBehaviour
{
    [SerializeField] private float initialVelocityX = 5f;
    [SerializeField] private float initialVelocityY = 10f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private ParticleSystem rocketParticleSystem;  // Particle system for bursts
    [SerializeField] private float rotationAngle = 40f;            // Rotation angle during burst
    private Vector3 velocity;
    private bool isBursting = false;


    [SerializeField] private GameObject subbulletPrefab;  // Prefab for subbullets
    [SerializeField] private float subbulletSpeed = 5f;
    void Start()
    {
        velocity = new Vector3(initialVelocityX, initialVelocityY, 0);
        StartCoroutine(BurstRoutine());
    }

    private void FixedUpdate()
    {
        ApplyParabolicMotion();
        AlignWithMovement();
    }

    private void ApplyParabolicMotion()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        velocity.y -= gravity * Time.fixedDeltaTime;  // Apply gravity to vertical velocity
    }

    private void AlignWithMovement()
    {
        if (!isBursting)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    private void SpawnSubbullets()
    {
        Vector3 spawnPosition = transform.position;

        // Spawn left-moving subbullet
        GameObject leftSubbullet = Instantiate(subbulletPrefab, spawnPosition, Quaternion.identity);
        leftSubbullet.GetComponent<subbulletTwoMovement>().initialXspeed = subbulletSpeed;
        leftSubbullet.SetActive(true);
        // Spawn middle subbullet
        GameObject middleSubbullet = Instantiate(subbulletPrefab, spawnPosition, Quaternion.identity);
        middleSubbullet.GetComponent<subbulletTwoMovement>().initialXspeed = 0;
        middleSubbullet.SetActive(true);
        // Spawn right-moving subbullet
        GameObject rightSubbullet = Instantiate(subbulletPrefab, spawnPosition, Quaternion.identity);
        rightSubbullet.GetComponent<subbulletTwoMovement>().initialXspeed = -subbulletSpeed;
        rightSubbullet.SetActive(true);
    }
    private IEnumerator BurstRoutine()
    {
        for (int burstCount = 0; burstCount < 3; burstCount++)
        {
            float timeToApex = initialVelocityY / gravity;
            yield return new WaitForSeconds(timeToApex * 0.8f);  // Wait until 4/5 down from apex

            yield return RotateAndFall(0.3f);  // Rotate 30 degrees
            TriggerBurst();  // Simulate burst

            velocity.y = Mathf.Max(initialVelocityY * Mathf.Pow(0.6f, burstCount + 1), 0);  // Reduced vertical velocity
            initialVelocityY *= 0.6f;  // Dampen next burst power
        }

        PerformFinalBurstAndDestroy();
    }

    private IEnumerator RotateAndFall(float duration)
    {
        isBursting = true;
        Quaternion initialRotation = transform.rotation;
        Quaternion burstRotation = Quaternion.Euler(0, 0, rotationAngle);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initialRotation, burstRotation, elapsed / duration);
            yield return null;
        }

        isBursting = false;
    }

    private void TriggerBurst()
    {
        if (rocketParticleSystem != null)
        {
            rocketParticleSystem.Play();
        }
    }

    private void PerformFinalBurstAndDestroy()
    {
        SpawnSubbullets();
        Destroy(gameObject);
    }
}