using UnityEngine;
using System.Collections.Generic;

public class ParticleEmitter : MonoBehaviour
{
    public static ParticleEmitter Instance { get; private set; }  // Singleton instance

    public GameObject particlePrefab;      // Prefab for the particles
    public List<StackController> stacks;   // List of StackController objects
    public float sigma = 2f;               // Standard deviation for the Gaussian distribution
    public List<float> probabilities;     // Precomputed probabilities for stack selection
    private float totalProbability;        // Total of all probabilities, for normalizing
    public Color Particle1Color;
    public Color Particle2Color;
    public Color Particle3Color;
    public Color Particle4Color;
    public Color Particle5Color;
    public float ParticleHeight= 0.06f;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Ensure only one instance
            return;
        }
    }

    void Start()
    {
        ComputeStackSelectionProbabilities();  // Precompute stack selection probabilities
    }

    // Precompute the stack selection probabilities based on Gaussian distribution
    void ComputeStackSelectionProbabilities()
    {
        probabilities = new List<float>();
        totalProbability = 0f;

        float mean = stacks.Count / 2f;  // Center of the stack list

        for (int i = 0; i < stacks.Count; i++)
        {
            float distance = Mathf.Abs(i - mean);
            float probability = Mathf.Exp(-(distance * distance) / (2f * sigma * sigma));
            probabilities.Add(probability);
            totalProbability += probability;
        }

        for (int i = 0; i < probabilities.Count; i++)
        {
            probabilities[i] /= totalProbability;
        }
    }

    // Static method to spawn a particle at a specified position with size
    public static void SpawnParticle(Vector3 position, int size)
    {
        if (Instance == null)
        {
            Debug.LogError("ParticleEmitter instance not found in the scene.");
            return;
        }

        // Select a stack using the precomputed probabilities
        StackController selectedStack = Instance.SelectStackBasedOnProbability();

        // Create the particle
        GameObject newParticle = Instantiate(Instance.particlePrefab, position, Quaternion.identity);

        // Set particle behavior properties
        ParticleBehavior particleBehavior = newParticle.GetComponent<ParticleBehavior>();
        if (particleBehavior != null)
        {
            particleBehavior.SetTargetStack(selectedStack);
            particleBehavior.Size = size;  // Set particle size
        }

        // Set color based on size
        Color selectedColor;
        switch (size)
        {
            case 1:
                selectedColor = Instance.Particle1Color;
                break;
            case 2:
                selectedColor = Instance.Particle2Color;
                break;
            case 3:
                selectedColor = Instance.Particle3Color;
                break;
            case 4:
                selectedColor = Instance.Particle4Color;
                break;
            case 5:
                selectedColor = Instance.Particle5Color;
                break;
            default:
                selectedColor = Color.white;  // Default color
                break;
        }

        // Apply color to SpriteRenderer
        SpriteRenderer spriteRenderer = newParticle.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = selectedColor;
        }

        // Apply color to TrailRenderer
        TrailRenderer trailRenderer = newParticle.GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.startColor = selectedColor;
            trailRenderer.endColor = selectedColor;  // Set both to same color to avoid fading
        }

        newParticle.SetActive(true);
    }
    // Select a stack based on the precomputed probabilities
    public StackController SelectStackBasedOnProbability()
    {
        float randomValue = Random.value;
        float cumulativeProbability = 0f;

        for (int i = 0; i < stacks.Count; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return stacks[i];
            }
        }

        return stacks[stacks.Count - 1];  // Fallback for rounding errors
    }
}
