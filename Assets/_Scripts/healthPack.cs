using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [Header("Health Pack Settings")]
    [Tooltip("Amount of health restored.")]
    public int healthRestoreAmount = 25;

    [Tooltip("Layer to ignore collisions (e.g., Enemy).")]
    public LayerMask ignoreLayer;

    private MeshRenderer meshRenderer;
    private Collider packCollider;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        packCollider = GetComponent<Collider>();

        if (meshRenderer == null || packCollider == null)
        {
            Debug.LogError("Health Pack is missing required components.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider is on the ignore layer
        if ((ignoreLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            return; // Ignore interaction with this layer
        }

        // Check if the collider belongs to an entity with CharacterActor
        CharacterActor character = other.GetComponent<CharacterActor>();

        if (character != null && !character.isDead && character.CurrentHealth() < character.maxHealth)
        {
            character.RestoreHealth(healthRestoreAmount);

            // Destroy the health pack object after use
            Destroy(gameObject);
        }
    }
}
