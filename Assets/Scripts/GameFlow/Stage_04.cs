using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stage_04 : GameStage
{
    public DestinationChannel destinationChannel;
    public ProducerChannel producerChannel;
    public int successesToTransition;
    public Inventory playerInventory;
    public PackageType[] allPackageTypes;

    
    public int numDeliveriesInThisStage = 0;
    public int spawnCloudsAfterStageSuccessCount;

    // This stage is activated if the player successfully completes a dropoff in Stage_02
    private void SuccessfulDropoffCountChanged(AreaOfInterest arg0, int successCount)
    {
        if (canTransitionFrom.Contains(gameDirector.currentStage) && successCount >= successesToTransition)
            gameDirector.TransitionTo(this);
        else if (gameDirector.currentStage == this)
        {
            numDeliveriesInThisStage++;
            KeepSpawningDestinations(GetRandomPackageType());
        }
    }

    private void OnEnable()
    {
        destinationChannel.SuccessfulDropoff += SuccessfulDropoffCountChanged;
        destinationChannel.MissedDropoff += MissedDropoff;
        destinationChannel.IncorrectDropoff += IncorrectDropoff;
        
        producerChannel.ProducerBecameAvailable += ProducerBecameAvailable;
    }

    private void OnDisable()
    {
        destinationChannel.SuccessfulDropoff -= SuccessfulDropoffCountChanged;
        destinationChannel.MissedDropoff -= MissedDropoff;
        destinationChannel.IncorrectDropoff -= IncorrectDropoff;
        
        producerChannel.ProducerBecameAvailable -= ProducerBecameAvailable;
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
        gameDirector.SpawnDestinationIfPossible(packageType);
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
        {
            numDeliveriesInThisStage++;
            KeepSpawningDestinations(destination.PackageType);
        }
    }

    private void MissedDropoff(Destination destination, int i)
    {
        if (gameDirector.currentStage == this)
        {
            numDeliveriesInThisStage++;
            KeepSpawningDestinations(destination.PackageType);
        }
    }
    
    private void ProducerBecameAvailable(Producer producer)
    {
        if (gameDirector.currentStage != this)
            return;
        
        var packageType = GetRandomPackageType();
        gameDirector.BeginCreatingPackage(producer, packageType);
        gameDirector.SpawnDestinationIfPossible(packageType);

        if (numDeliveriesInThisStage >= spawnCloudsAfterStageSuccessCount)
            gameDirector.SpawnCloud();
    }

    private void KeepSpawningDestinations(PackageType packageType)
    {
        gameDirector.SpawnDestinationIfPossible(packageType);
    }
}
