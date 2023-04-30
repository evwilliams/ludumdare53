using UnityEngine;
using UnityEngine.InputSystem;

public class Stage_01 : GameStage
{
    public InputChannel inputChannel;
    public DestinationChannel destinationChannel;
    private PackageType startingPackageType;

    // This stage is activated if the player moves during the tutorial stage
    private void MovementPerformed(InputAction.CallbackContext arg0, Vector2 arg1)
    {
        if (canTransitionFrom.Contains(gameDirector.currentStage))
            gameDirector.TransitionTo(this);
    }

    private void OnEnable()
    {
        inputChannel.MovementPerformed += MovementPerformed;
        destinationChannel.MissedDropoff += MissedDropoff;
    }

    private void OnDisable()
    {
        inputChannel.MovementPerformed -= MovementPerformed;
        destinationChannel.MissedDropoff -= MissedDropoff;
    }

    public override void OnStageEnter()
    {
        Debug.Log($"Entering {name}");
        var firstDestination = gameDirector.GetDestination(0);
        startingPackageType = firstDestination.PackageType;
        firstDestination.StartTimer(GameDirector.DestinationCountdownTime);
    }

    public override void OnStageExit()
    {
        Debug.Log($"Exiting {name}");
        enabled = false;
        gameObject.SetActive(false);
    }
    
    private void MissedDropoff(Destination destination, int missCount)
    {
        if (gameDirector.currentStage == this)
            KeepSpawningDestinations();
    }

    private void KeepSpawningDestinations()
    {
        Debug.Log($"{name} spawning of type {startingPackageType}");
        gameDirector.SpawnDestinationWherePossible(startingPackageType);
    }
}
