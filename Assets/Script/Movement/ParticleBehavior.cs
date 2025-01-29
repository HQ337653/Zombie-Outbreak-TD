using UnityEngine;

public class ParticleBehavior : MonoBehaviour
{
    private StackController targetStack;
    private Vector3 startPosition;
    private float targetX;

    [SerializeField] float extraHeight = 1f; // Extra height above the stack
    public int Size = 1;

    private const float gravity = 30f;      // Downward acceleration
    private float horizontalSpeed;
    private float initialVerticalSpeed;
    private float totalTime;
    private float timeAlive = 0f;

    // Set the target stack and precompute motion parameters
    public void SetTargetStack(StackController stack)
    {
        float randomOffset = 0.1f * Random.value;
        transform.position += new Vector3(randomOffset, randomOffset, 0);  // Adjust position slightly
       TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.time += randomOffset/2;  // Adjust trail time slightly
        }

        extraHeight *= (1+Random.value/2);
        targetStack = stack;
        startPosition = transform.position;
        targetX = stack.transform.position.x;

        float horizontalDistance = targetX - startPosition.x;
        float peakHeight = Mathf.Max(startPosition.y + extraHeight, stack.GetHeight() + extraHeight);

        float timeToPeak = Mathf.Sqrt(2 * (peakHeight - startPosition.y) / gravity);
        float timeFromPeak = Mathf.Sqrt(2 * (peakHeight - stack.GetHeight()) / gravity);
        totalTime = timeToPeak + timeFromPeak;

        initialVerticalSpeed = gravity * timeToPeak;
        horizontalSpeed = horizontalDistance / totalTime;

        // Automatically destroy the particle after the calculated flight time
    }

    void Update()
    {
        if (timeAlive >= totalTime)
        {
            targetStack.IncreaseHeight(Size);  // Increase stack height when time is up
            Destroy(gameObject);
            return;
        }

        float newX = startPosition.x + horizontalSpeed * timeAlive;
        float newY = startPosition.y + initialVerticalSpeed * timeAlive - 0.5f * gravity * timeAlive * timeAlive;

        transform.position = new Vector3(newX, newY, transform.position.z);
        timeAlive += Time.deltaTime;
    }
}
