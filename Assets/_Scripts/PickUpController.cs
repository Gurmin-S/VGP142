using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] private Attack attack;

    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, weapon;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Start()
    {
        if (!equipped)
        {
            attack.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        else
        {
            attack.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;

        // Pickup logic
        if (!equipped && distanceToPlayer.magnitude < pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            PickUp();
        }

        // Drop logic
        if (equipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        // Set the weapon's parent to the player and reset its local position/rotation
        transform.SetParent(weapon);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(new Vector3(-180f, 0f, 0f)); // Set the rotation correctly
        transform.localScale = Vector3.one;

        // Set the Rigidbody to kinematic to prevent physics interaction
        rb.isKinematic = true;
        coll.isTrigger = true;

        // Enable attack logic
        attack.enabled = true;
    }


    private void Drop()
    {
        equipped = false;
        slotFull = false;

        // Detach the weapon from the player
        transform.SetParent(null);

        // Re-enable physics and disable the weapon attack logic
        rb.isKinematic = false; // Disable kinematic to allow physics interaction
        coll.isTrigger = false;
        attack.enabled = false;

        // Get the camera's forward direction (ignoring the vertical component)
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;

        // Apply force based on the camera's facing direction
        rb.AddForce(cameraForward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(Camera.main.transform.up * dropUpwardForce, ForceMode.Impulse);

        // Add random spin to the weapon
        float randomSpin = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(randomSpin, randomSpin, randomSpin) * 10);
    }


}
