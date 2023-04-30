using UnityEngine;
using UnityEngine.InputSystem;

public class Stage_03 : GameStage
{
    public InputChannel inputChannel;
    public DestinationChannel destinationChannel;
    
    public Inventory playerInventory;
    public PackageType packageType1;
    public PackageType packageType2;

    private void OnEnable()
    {
        inputChannel.MovementStarted += MovementStarted;
        destinationChannel.MissedDropoff += MissedDropoff;
        destinationChannel.IncorrectDropoff += IncorrectDropoff;
    }

    private void OnDisable()
    {
        inputChannel.MovementStarted -= MovementStarted;
        destinationChannel.MissedDropoff -= MissedDropoff;
        destinationChannel.IncorrectDropoff -= IncorrectDropoff;
    }
    
    private void MovementStarted(InputAction.CallbackContext arg0, Vector2 arg1)
    {
        if (canTransitionFrom.Contains(gameDirector.currentStage))
            gameDirector.TransitionTo(this);
    }

    public override void OnStageEnter()
    {
        Debug.Log($"Entering {name}");
        playerInventory.maxPackagesAllowed = 2;
        
        SetupSourceAndDestination(packageType1);
        SetupSourceAndDestination(packageType2);
    }

    private void SetupSourceAndDestination(PackageType packageType)
    {
        gameDirector.SpawnDestinationIfPossible(packageType);
        gameDirector.BeginCreatingPackage(packageType == packageType1 ? 0 : 1, packageType);
    }

    public override void OnStageExit()
    {
        Debug.Log($"Exiting {name}");
        enabled = false;
        gameObject.SetActive(false);
    }

    /*
     * Continue spawning until the player triggers the next stage
     */
    private void IncorrectDropoff(Destination destination, int i)
    {
        if (gameDirector.currentStage == this)
            KeepSpawningDestinations(destination.PackageType);
    }

    private void MissedDropoff(Destination destination, int i)
    {
        if (gameDirector.currentStage == this)
            KeepSpawningDestinations(destination.PackageType);
    }

    private void KeepSpawningDestinations(PackageType missedType)
    {
        SetupSourceAndDestination(missedType);
    }
}
