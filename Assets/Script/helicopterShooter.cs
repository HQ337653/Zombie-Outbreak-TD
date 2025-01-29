using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helicopterShooter : MonoBehaviour
{
    public GameObject bulletPrefab;  // The bullet to spawn
    public float fireRatePerMinute = 60f;  // Bullets per minute
    public float waitTime;
    private float fireInterval;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fireInterval = 60f / fireRatePerMinute;  // Convert rate to interval in seconds
        StartCoroutine(FireBullets());
    }

    private IEnumerator FireBullets()
    {
        yield return null;yield return new WaitForSeconds(waitTime);
        while (true)
        {
            
            SpawnBullet();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void SpawnBullet()
    {
        var g = Instantiate(bulletPrefab, transform.position, transform.rotation);
        g.SetActive(true);
        int direction = (transform.rotation.y==0) ? 1 : -1;
        g.GetComponent<normalBulletMovement>().WeaponVelocity=new Vector2(6*direction, 0);
    }
}
