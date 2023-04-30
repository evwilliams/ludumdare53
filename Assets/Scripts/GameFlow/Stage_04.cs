using UnityEngine;
using UnityEngine.InputSystem;

public class Stage_04 : GameStage
{
    public DestinationChannel destinationChannel;
    public int successesToTransition;
    public Inventory playerInventory;
    public PackageType[] allPackageTypes;

    // This stage is activated if the player successfully completes a dropoff in Stage_02
    private void SuccessfulDropoffCountChanged(AreaOfInterest arg0, int successCount)
    {
        if (canTransitionFrom.Contains(gameDirector.currentStage) && successCount >= successesToTransition)
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
    
    PackageType GetRandomPackageType()
    {
        return allPackageTypes[Random.Range (0, allPackageTypes.Length)];
    }

    public override void OnStageEnter()
    {
        Debug.Log($"Entering {name}");
        playerInventory.maxPackagesAllowed = 2;
        
        SetupSourceAndDestination(0, GetRandomPackageType());
        SetupSourceAndDestination(1, GetRandomPackageType());
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
        SetupSourceAndDestination(0, missedType);
    }
}
