using UnityEngine;
using UnityEngine.InputSystem;

public class Stage_01 : GameStage
{
    public InputChannel inputChannel;

    // This stage is activated if the player moves during the tutorial stage
    private void MovementPerformed(InputAction.CallbackContext arg0, Vector2 arg1)
    {
        if (canTransitionFrom.Contains(gameDirector.currentStage))
            gameDirector.TransitionTo(this);
    }

    private void OnEnable()
    {
        inputChannel.MovementPerformed += MovementPerformed;
    }

    private void OnDisable()
    {
        inputChannel.MovementPerformed -= MovementPerformed;
    }

    public override void OnStageEnter()
    {
        Debug.Log($"Entering {name}");
        var firstDestination = gameDirector.GetDestination(0);
        firstDestination.StartTimer(GameDirector.DestinationCountdownTime);
    }

    public override void OnStageExit()
    {
        Debug.Log($"Exiting {name}");
        enabled = false;
    }
}
