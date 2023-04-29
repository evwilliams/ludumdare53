using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameDirector : MonoBehaviour
{
    public const int DestinationCountdownTime = 9;
    public const int StartingRating = 4;
    
    public Inventory playerInventory;
    public List<AreaOfInterest> sources = new();
    private List<AreaOfInterest> destinations = new();

    public AreaOfInterest destinationPrefab;
    public List<Transform> destinationLocations = new();

    public AOIChannel pickupChannel;
    public AOIChannel dropoffChannel;

    public Package packagePrefab;

    public PackageType[] packageTypes;

    public GameStage startingStage;
    public GameStage currentStage;

    private float _starRating;
    public float StarRating
    {
        get => _starRating;
    }
    
    public float ratingSum = 0;
    public int numRatingsReceived = 0;
    public FloatChannel starRatingChannel;
    
    private void OnEnable()
    {
        pickupChannel.Entered += PickupEntered;
        dropoffChannel.Entered += DropoffEntered;
        dropoffChannel.TimerExpired += DropoffTimerExpired;
    }

    private void DropoffTimerExpired(AreaOfInterest destination)
    {
        Debug.Log($"{destination.name}'s timer expired");
        // AssignNewType(destination);
    }

    private void Start()
    {
        InitRatingInfo();
        TransitionTo(startingStage);
    }

    private void InitRatingInfo()
    {
        ratingSum = StartingRating;
        numRatingsReceived = 1;
        UpdateStarRating();
    }

    public void RateDelivery(float rating)
    {
        ratingSum += rating;
        numRatingsReceived++;
        UpdateStarRating();
    } 

    public void UpdateStarRating()
    {
        _starRating = ratingSum / numRatingsReceived;
        starRatingChannel.ValueChanged?.Invoke(_starRating);
    }

    public void TransitionTo(GameStage gameStage)
    {
        if (currentStage)
            currentStage.OnStageExit();
        currentStage = gameStage;
        currentStage.OnStageEnter();
    }

    public AreaOfInterest SpawnDestination(int spawnNumber)
    {
        return SpawnDestination(destinationLocations[spawnNumber]);
    }
    
    public AreaOfInterest SpawnDestination(Transform spawnLocation)
    {
        var destination = Instantiate(destinationPrefab, spawnLocation.position, Quaternion.identity);
        destinations.Add(destination);
        return destination;
    }

    public AreaOfInterest GetDestination(int index)
    {
        return destinations[index];
    }
    
    public AreaOfInterest GetSource(int index)
    {
        return sources[index];
    }

    // public void AssignNewType(AreaOfInterest area)
    // {
    //     area.SetPackageType(GetRandomPackageType());
    //     area.StartTimer(DestinationCountdownTime);
    // }

    PackageType GetRandomPackageType()
    {
        return packageTypes[Random.Range (0, packageTypes.Length)];
    }

    private void OnDisable()
    {
        pickupChannel.Entered -= PickupEntered;
        dropoffChannel.Entered -= DropoffEntered;
        dropoffChannel.TimerExpired -= DropoffTimerExpired;
    }

    private void PickupEntered(AreaOfInterest area)
    {
        // Only one package for now
        if (playerInventory.HasPackage())
            return;
        
        // Debug.Log($"Pickup entered: {area.name}");
        var package = Instantiate(packagePrefab);
        package.SetPackageType(area.PackageType);
        playerInventory.TryPickup(package);
    }

    private void DropoffEntered(AreaOfInterest area)
    {
        if (!playerInventory.HasPackage())
            return;
        
        // Debug.Log($"Dropoff entered: {area.name}");
        var pack = playerInventory.GetPackage(0);
        if (pack.PackageType == area.PackageType)
        {
            playerInventory.TryDropoff();
            RateDelivery(5);
        }
    }
}
