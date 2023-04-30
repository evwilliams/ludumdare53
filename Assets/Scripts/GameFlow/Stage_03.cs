using UnityEngine;
using UnityEngine.InputSystem;

public class Stage_03 : GameStage
{
    public DestinationChannel destinationChannel;

    public Inventory playerInventory;
    public PackageType[] packageTypes; // Expects at least 3

    // This stage is activated if the player successfully completes a dropoff in Stage_02
    private void SuccessfulDropoffCountChanged(AreaOfInterest arg0, int successCount)
    {
        if (canTransitionFrom.Contains(gameDirector.currentStage) && successCount > 0)
            gameDirector.TransitionTo(this);
    }

    private void OnEnable()
    {
        destinationChannel.SuccessfulDropoff += SuccessfulDropoffCountChanged;
        destinationChannel.MissedDropoff += MissedDropoff;
        destinationChannel.IncorrectDropoff += IncorrectDropoff;
    }

    private void OnDisable()
    {
        destinationChannel.SuccessfulDropoff -= SuccessfulDropoffCountChanged;
        destinationChannel.MissedDropoff -= MissedDropoff;
        destinationChannel.IncorrectDropoff -= IncorrectDropoff;
    }

    public override void OnStageEnter()
    {
        Debug.Log($"Entering {name}");
        playerInventory.allowTwoPackages = true;
        
        SetupSourceAndDestination(0, packageTypes[1]);
        SetupSourceAndDestination(1, packageTypes[2]);
    }

    private void SetupSourceAndDestination(int sourceIndex, PackageType packageType)
    {
        gameDirector.SpawnDestinationWherePossible(packageType);
        gameDirector.BeginCreatingPackage(sourceIndex, packageType);
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
        gameDirector.SpawnDestinationWherePossible(missedType);
    }
}
