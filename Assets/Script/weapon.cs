using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public GameObject bulletPrefab;  // The bullet to spawn
    public float fireRatePerMinute = 60f;  // Bullets per minute

    private float fireInterval;

    void Start()
    {
        fireInterval = 60f / fireRatePerMinute;  // Convert rate to interval in seconds
        StartCoroutine(FireBullets());
    }

    private IEnumerator FireBullets()
    {
        while (true)
        {
            SpawnBullet();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void SpawnBullet()
    {
       var g= Instantiate(bulletPrefab, transform.position, transform.rotation);
        g.SetActive(true);
    }
}
