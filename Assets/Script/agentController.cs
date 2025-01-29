using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentController : MonoBehaviour
{
    public int capacity = 5;
    [SerializeField] private int currentlyCarry = 0;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveBackSpeed = 2f;
    private Rigidbody2D rb;
    private StackController targetStack;

    [SerializeField] private SpriteRenderer spriteRenderer;  // Reference to SpriteRenderer
    [SerializeField] private Sprite spriteOption1;          // First sprite option
    [SerializeField] private Sprite spriteOption2;          // Second sprite option
    [SerializeField] private Sprite spriteOption3;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ControlLoop());  // Start the loop to continuously find and pick from stacks
    }

    private IEnumerator ControlLoop()
    {
        while (true)
        {
            yield return MoveToRandomStackAndPick();
            yield return MoveBackAndWait();
        }
    }

    private IEnumerator MoveToRandomStackAndPick()
    {
        targetStack = ParticleEmitter.Instance.SelectStackBasedOnProbability();
        Vector2 targetPosition = new Vector2(targetStack.transform.position.x, rb.position.y);

        while (Mathf.Abs(rb.position.x - targetPosition.x) > 0.1f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }

        PickFromStack();
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator MoveBackAndWait()
    {
        Vector2 leftPosition = new Vector2(-10f, rb.position.y);  // Arbitrary position to move to the left

        while (rb.position.x > leftPosition.x)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, leftPosition, moveBackSpeed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        EmptyCurrentStack();
        yield return new WaitForSeconds(2f);
    }

    private void PickFromStack()
    {
        int amountToPick = Mathf.Min(capacity - currentlyCarry, targetStack.StackSize);
        if (amountToPick > 0)
        {
            currentlyCarry += amountToPick;
            targetStack.IncreaseHeight(-amountToPick);
            IncreaseDisplayHeight(amountToPick);
        }
    }

    private void EmptyCurrentStack()
    {
        
        IncreaseDisplayHeight(-currentlyCarry);
        currentlyCarry = 0;
    }

    public void IncreaseDisplayHeight(int amount)
    {
        float visualIncrement = amount * ParticleEmitter.Instance.ParticleHeight;

        // Update stack height

        // Set a random sprite
        AssignRandomSprite();

        // Adjust the SpriteRenderer's size by changing the height
        Vector2 size = spriteRenderer.size;
        size.y += visualIncrement;  // Increase only the height
        spriteRenderer.size = size;

        // Move the stack upward by half the height change
        spriteRenderer.transform.position = new Vector3(spriteRenderer.transform.position.x, spriteRenderer.transform.position.y + visualIncrement / 2 / 5, spriteRenderer.transform.position.z);
    }
    private void AssignRandomSprite()
    {
        Sprite[] sprites = { spriteOption1, spriteOption2, spriteOption3 };
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
