using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stage_01 : GameStage
{
    public InputChannel inputChannel;
    private AreaOfInterest firstDestination;

    private void OnEnable()
    {
        inputChannel.MovementPerformed += MovementPerformed;
    }

    private void OnDisable()
    {
        inputChannel.MovementPerformed -= MovementPerformed;
    }

    private void MovementPerformed(InputAction.CallbackContext arg0, Vector2 arg1)
    {
        if (canTransitionFrom.Contains(gameDirector.currentStage))
            gameDirector.TransitionTo(this);
    }

    public override void OnStageEnter()
    {
        Debug.Log($"Entering {name}");
        firstDestination = gameDirector.GetDestination(0);
        firstDestination.StartTimer(GameDirector.DestinationCountdownTime);
    }

    public override void OnStageExit()
    {
        Debug.Log($"Exiting {name}");
        enabled = false;
    }
}
