using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Channels/PlayerInput")]
public class InputChannel : ScriptableObject
{
    public UnityAction<InputAction.CallbackContext, Vector2> MovementPerformed;
}
