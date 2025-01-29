using UnityEngine;

public class FlipMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;  // Speed of movement
    private bool movingRight = true;  // Direction flag

    private void Update()
    {
        // Move right or left based on the direction
        float direction = 1;
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // Check if a boundary is reached (for example, arbitrary x positions)
        if (transform.position.x > 7f && movingRight)
        {
            FlipDirection(false);  // Move left when reaching the right boundary
        }
        else if (transform.position.x < -3f && !movingRight)
        {
            FlipDirection(true);  // Move right when reaching the left boundary
        }
    }

    private void FlipDirection(bool moveRight)
    {
        movingRight = moveRight;
        float rotationY = moveRight ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);  // Rotate around Y to flip
    }
}
