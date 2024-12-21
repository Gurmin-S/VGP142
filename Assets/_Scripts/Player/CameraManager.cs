using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target; // The player or target object
    public float distance = 5.0f; // Distance from the player
    public float mouseSensitivity = 100f;
    public float rotationSmoothTime = 0.12f;
    public Vector3 offset = new Vector3(0f, 2f, 0f); // Offset to move the camera higher

    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;
    private float yaw;
    private float pitch;

    void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize the yaw and pitch to the camera's initial rotation
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        Orbit();
    }

    private void Orbit()
    {
        // Get mouse input
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Clamp the pitch to avoid flipping the camera
        pitch = Mathf.Clamp(pitch, -35f, 60f);

        // Smooth the rotation
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

        // Apply the rotation to the camera
        transform.eulerAngles = currentRotation;

        // Calculate the new camera position with offset and distance
        transform.position = target.position + offset - transform.forward * distance;
    }
}
