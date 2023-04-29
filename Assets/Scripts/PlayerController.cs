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
    public SpriteRenderer spriteRenderer;

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

    private void MovementPerformed(InputAction.CallbackContext obj)
    {
        var inputVec = obj.ReadValue<Vector2>();
        moveInput = new Vector3(inputVec.x, 0, inputVec.y);
    }

    private void Update()
    {
        if (moveInput != Vector3.zero)
        {
            characterController.Move(moveInput * (moveSpeed * Time.deltaTime));
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }
}
