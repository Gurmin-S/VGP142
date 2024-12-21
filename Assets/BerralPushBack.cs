using UnityEngine;

public class BarrelPushback : MonoBehaviour
{
    [Header("Pushback Settings")]
    public float pushbackForce = 5f; // The force to apply when a CharacterController touches the prefab
    public float pushbackDuration = 1f; // Duration over which the pushback happens
    private Vector3 pushbackVelocity; // Current velocity of the pushback
    private float pushbackTime = 0f; // Time elapsed since the pushback started

    private void OnCollisionStay(Collision collision)
    {
        // Check if the object that collided is a CharacterController
        CharacterController characterController = collision.gameObject.GetComponent<CharacterController>();
        if (characterController != null)
        {
            // Calculate the direction to push the character back
            Vector3 pushDirection = collision.transform.position - transform.position;
            pushDirection.y = 0; // Keep the pushback horizontal
            pushDirection.Normalize(); // Normalize the direction

            // Gradually apply the pushback over time
            if (pushbackTime < pushbackDuration)
            {
                // Increase the pushback time and calculate the velocity for smooth application
                pushbackTime += Time.deltaTime;
                pushbackVelocity = Vector3.Lerp(pushbackVelocity, pushDirection * pushbackForce, pushbackTime / pushbackDuration);
            }
            else
            {
                // Once the pushback duration is reached, apply the full force
                pushbackVelocity = pushDirection * pushbackForce;
            }

            // Apply the smooth pushback velocity to the character controller
            characterController.Move(pushbackVelocity * Time.deltaTime);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset pushback when the character exits the collision
        if (collision.gameObject.GetComponent<CharacterController>() != null)
        {
            pushbackVelocity = Vector3.zero;
            pushbackTime = 0f;
        }
    }
}
