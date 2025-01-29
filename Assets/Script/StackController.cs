
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] private float stackHeight = 0.5f;  // Initial Y-position of the stack
    public int StackSize;

    [SerializeField] private SpriteRenderer spriteRenderer;  // Reference to SpriteRenderer
    [SerializeField] private Sprite spriteOption1;          // First sprite option
    [SerializeField] private Sprite spriteOption2;          // Second sprite option
    [SerializeField] private Sprite spriteOption3;          // Third sprite option

    private void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

    // Increase the stack's height

    public void IncreaseHeight(int amount)
    {
        StackSize += amount;
        float visualIncrement = amount * ParticleEmitter.Instance.ParticleHeight;

        // Update stack height
        stackHeight += visualIncrement / 2 / 2.5f;

        // Set a random sprite
        AssignRandomSprite();

        // Adjust the SpriteRenderer's size by changing the height
        Vector2 size = spriteRenderer.size;
        size.y += visualIncrement;  // Increase only the height
        spriteRenderer.size = size;

        // Move the stack upward by half the height change
        transform.position = new Vector3(transform.position.x, transform.position.y + visualIncrement / 2/5, transform.position.z);
    }

    // Assigns a random sprite from the options
    private void AssignRandomSprite()
    {
        Sprite[] sprites = { spriteOption1, spriteOption2, spriteOption3 };
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    // Get the current top height of the stack
    public float GetHeight()
    {
        return stackHeight;
    }
}
