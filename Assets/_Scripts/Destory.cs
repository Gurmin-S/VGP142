using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the projectile upon collision
        Destroy(gameObject);
    }
}
