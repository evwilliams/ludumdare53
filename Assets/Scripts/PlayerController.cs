using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public CharacterController characterController;
    public PlayerControls playerInput;
    public Transform bodyTransform;

    public InputChannel inputChannel;

    private Vector3 moveInput = Vector3.zero;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new();
        playerInput.CharacterControls.Enable();
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Movement.performed += MovementPerformed;
        playerInput.CharacterControls.Movement.canceled += MovementCanceled;
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Movement.performed -= MovementPerformed;
        playerInput.CharacterControls.Movement.canceled -= MovementCanceled;
    }

    private void MovementCanceled(InputAction.CallbackContext obj)
    {
        moveInput = Vector3.zero;
    }

    private void MovementPerformed(InputAction.CallbackContext context)
    {
        var inputVec = context.ReadValue<Vector2>();
        moveInput = new Vector3(inputVec.x, 0, inputVec.y);
        
        inputChannel.MovementPerformed?.Invoke(context, moveInput);
    }

    private void Update()
    {
        if (moveInput != Vector3.zero)
        {
            characterController.Move(moveInput * (moveSpeed * Time.deltaTime));
            var bodyTransformLocalScale = bodyTransform.localScale;
            bodyTransformLocalScale.x = moveInput.x > 0 ? 1 : -1;
            bodyTransform.localScale = bodyTransformLocalScale;
        }
    }
}
