using UnityEngine;
using UnityEngine.InputSystem;

public class Stage_02 : GameStage
{
    public DestinationChannel destinationChannel;

    public PackageType secondPackageType;
    private PackageType _firstPackageType;


    // This stage is activated if the player successfully completes a dropoff in Stage_01
    private void SuccessfulDropoffCountChanged(Destination destination, int successCount)
    {
        if (canTransitionFrom.Contains(gameDirector.currentStage) && successCount > 0)
        {
            _firstPackageType = destination.PackageType;
            gameDirector.TransitionTo(this);
        }
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
        gameDirector.SpawnDestinationIfPossible(secondPackageType);
        gameDirector.BeginCreatingPackage(0, _firstPackageType);
        gameDirector.BeginCreatingPackage(1, secondPackageType);
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
        gameDirector.SpawnDestinationIfPossible(missedType);
    }
}
