using UnityEngine;

public class BouncingMovement : MonoBehaviour
{
    [SerializeField] private float initialVerticalSpeed = 10f;  // Initial vertical speed (controls the height)
    [SerializeField] private float horizontalSpeed = 5f;  // Constant horizontal speed
    [SerializeField] private int maxBounces = 2;  // Number of bounces before stopping
    private float gravity = 20f;  // Gravity simulation
    private int bounceCount = 0;  // Count of bounces

    private float verticalSpeed;  // Current vertical speed
   [SerializeField] GameObject end;

    private void Start()
    {
        verticalSpeed = initialVerticalSpeed;
    }

    private void Update()
    {
        if (bounceCount > maxBounces)
        {
            explode();
        }

        // Calculate horizontal movement
        transform.position += new Vector3(horizontalSpeed * Time.deltaTime, 0f, 0f);

        // Calculate vertical movement with gravity
        verticalSpeed -= gravity * Time.deltaTime;
        transform.position += new Vector3(0f, verticalSpeed * Time.deltaTime, 0f);

        // Check for ground collision (y = 0)
        if (transform.position.y <= 0f)
        {
            Bounce();
        }
    }
    private void explode()
    {
        var g = Instantiate(end);
        g.transform.position = transform.position;
        g.SetActive(true);
        Destroy(gameObject);  // Destroy the object after the final bounce
        return;
    }

    private void Bounce()
    {
        if (Random.value>=0.7)
        {
            explode();
        }
        bounceCount++;
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);  // Snap to ground level
        verticalSpeed = -verticalSpeed * 2f / 3f;  // Reverse and reduce vertical speed for bounce
    }
}
