using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float rotationSpeed = 720f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;
    public float movementSmoothing = 5f;
    public Camera playerCamera; // Reference to the Camera component
    public float normalFov = 50f; // Normal FOV
    public float sprintFov = 55f; // Sprint FOV
    public float lerpSpeed = 5f; // Speed at which the FOV changes
    public float AnimlerpSpeed = 5f;
    public Animator animator; // Public reference to the Animator component

    private CharacterController controller;
    private Transform cameraTransform;

    private float verticalVelocity;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isSprinting;

    private float lastLookRot;
    private float lastCamRot;
    private float lerp;

    private Inputs inputActions;

    private float targetFov; // Target FOV (to smoothly transition to)
    private float currentFov; // Current FOV (interpolated value)

    void Awake()
    {
        inputActions = new Inputs();
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        inputActions.controls.Enable();
        inputActions.controls.Jump.performed += OnJump;
        inputActions.controls.Sprint.performed += OnSprint;
        inputActions.controls.Sprint.canceled += OnSprintCanceled;
    }

    void OnDisable()
    {
        inputActions.controls.Jump.performed -= OnJump;
        inputActions.controls.Sprint.performed -= OnSprint;
        inputActions.controls.Sprint.canceled -= OnSprintCanceled;
        inputActions.controls.Disable();
    }

    void Start()
    {
        cameraTransform = Camera.main != null ? Camera.main.transform : null;
        playerCamera = Camera.main; // Set the player camera to the main camera
        if (cameraTransform == null)
        {
            Debug.LogError("Main Camera not found! Make sure your camera is tagged as 'MainCamera'.");
        }
        lastLookRot = transform.eulerAngles.y;
        lastCamRot = Camera.main.transform.eulerAngles.y;

        // Initial FOV set to normal
        currentFov = normalFov;
        targetFov = normalFov;
        playerCamera.fieldOfView = normalFov;
    }

    void Update()
    {
        if (cameraTransform != null)
        {
            MovePlayer();
            RotatePlayer();
        }

        // Smoothly adjust the FOV
        currentFov = Mathf.Lerp(currentFov, targetFov, Time.deltaTime * lerpSpeed);
        playerCamera.fieldOfView = currentFov;

        // Handle gravity
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Small negative value to keep grounded
        }

        // Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        // Apply movement (gravity + movement direction)
        Vector3 move = new Vector3(0, verticalVelocity, 0);
        controller.Move(move * Time.deltaTime);

        // Update animator parameter
        UpdateAnimatorParameters();
    }

    private void MovePlayer()
    {
        // Calculate movement based on input
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Determine the move direction based on input
        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;

        // Adjust movement speed based on sprinting
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        // Rotate the player towards the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Apply movement (horizontal)
        Vector3 move = moveDirection * currentSpeed + Vector3.up * verticalVelocity;
        controller.Move(move * Time.deltaTime);

        wasGrounded = controller.isGrounded;
    }

    private void RotatePlayer()
    {
        if (lerp >= 1) return;
        lerp += Time.deltaTime * rotationSpeed;

        // Smoothing for rotation
        float lookRot = Mathf.Lerp(lastLookRot, lastCamRot, lerp);
        Vector3 rotation = transform.eulerAngles;
        rotation.y = lookRot;
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void UpdateAnimatorParameters()
    {
        if (animator != null)
        {
            // Calculate movement speed (normalized)
            float targetSpeed = moveInput.sqrMagnitude > 0 ? (isSprinting ? 1f : 0.5f) : 0f;

            // Smoothly transition to the target speed
            float smoothSpeed = Mathf.Lerp(animator.GetFloat("Speed"), targetSpeed, Time.deltaTime * AnimlerpSpeed);

            // Set the new smoothed speed to the animator
            animator.SetFloat("Speed", smoothSpeed);

            // Update the isGrounded parameter
            animator.SetBool("isGrounded", controller.isGrounded);
        }
    }


    public void MoveStarted(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void MoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity); // Apply jump velocity
        }
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = true; // Set sprinting to true when the sprint button is pressed
        targetFov = sprintFov; // Set the target FOV to sprint FOV
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false; // Set sprinting to false when the sprint button is released
        targetFov = normalFov; // Reset FOV to normal FOV
    }
}
