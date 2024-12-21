using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    Inputs inputAction;
    public PlayerController controller;

    private void OnEnable()
    {
        inputAction = new Inputs();
        inputAction.Enable();

        // Ensure the controller is not null before subscribing to events
        if (controller != null)
        {
            inputAction.controls.Move.performed += controller.MoveStarted;
            inputAction.controls.Move.canceled += controller.MoveCanceled;
        }
        else
        {
            Debug.LogError("Controller is not assigned in InputManager.");
        }
    }

    private void OnDisable()
    {
        inputAction.Disable();

        // Ensure the controller is not null before unsubscribing from events
        if (controller != null)
        {
            inputAction.controls.Move.performed -= controller.MoveStarted;
            inputAction.controls.Move.canceled -= controller.MoveCanceled;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Optionally add any initialization logic for the Start method
    }

    // Update is called once per frame
    void Update()
    {
        // Optionally add any logic that needs to run each frame
    }
}
