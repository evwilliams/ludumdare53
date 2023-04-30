using UnityEngine;
using UnityEngine.InputSystem;

public class Stage_02 : GameStage
{
    public IntChannel successfulDropoffsChannel;
    public IntChannel missedDropoffsChannel;
    public IntChannel incorrectDropoffsChannel;

    public PackageType secondPackageType;


    // This stage is activated if the player successfully completes a dropoff in Stage_01
    private void SuccessfulDropoffCountChanged(int successCount)
    {
        if (canTransitionFrom.Contains(gameDirector.currentStage) && successCount > 0)
            gameDirector.TransitionTo(this);
    }

    private void OnEnable()
    {
        successfulDropoffsChannel.ValueChanged += SuccessfulDropoffCountChanged;
        missedDropoffsChannel.ValueChanged += MissedDropoff;
        incorrectDropoffsChannel.ValueChanged += IncorrectDropoff;
    }

    private void OnDisable()
    {
        successfulDropoffsChannel.ValueChanged -= SuccessfulDropoffCountChanged;
    }

    public override void OnStageEnter()
    {
        Debug.Log($"Entering {name}");
        gameDirector.SpawnDestination(1, secondPackageType);
        gameDirector.InstantlyCreatePackage(1, secondPackageType);
    }

    public override void OnStageExit()
    {
        Debug.Log($"Exiting {name}");
        enabled = false;
    }
    
    
    /*
     * Continue spawning until the player triggers the next stage
     */
    private void IncorrectDropoff(int arg0)
    {
        KeepSpawningDestinations();
    }

    private void MissedDropoff(int arg0)
    {
        KeepSpawningDestinations();
    }

    private void KeepSpawningDestinations()
    {
        gameDirector.SpawnDestinationWherePossible(secondPackageType);
    }
}
