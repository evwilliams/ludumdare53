using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameDirector : MonoBehaviour
{
    public const int DestinationCountdownTime = 9;
    public const int PackageCreationTime = 6;
    public const int StartingRating = 4;

    public const int RatingForTimingMiss = 0;
    
    public Inventory playerInventory;
    public List<Producer> sources = new();
    private List<Destination> _destinations = new();

    public Destination destinationPrefab;
    public List<SpawnPoint> destinationLocations = new();

    public AOIChannel pickupChannel;
    public AOIChannel dropoffChannel;

    public Package packagePrefab;

    public PackageType[] packageTypes;

    public GameStage startingStage;
    public GameStage currentStage;

    private int _successfulDropoffs = 0;
    public int SuccessfulDropoffs
    {
        get => _successfulDropoffs;
    }
    public IntChannel successfulDropoffsChannel;
    
    
    
    private int _missedDropoffs = 0;
    public int MissedDropoffs
    {
        get => _missedDropoffs;
    }
    public IntChannel missedDropoffsChannel;
    
    private int _incorrectDropoffs = 0;
    public int IncorrectDropoffs
    {
        get => _incorrectDropoffs;
    }
    public IntChannel incorrectDropoffsChannel;
    
    
    private float _starRating;
    public float StarRating
    {
        get => _starRating;
    }
    
    private float _ratingSum = 0;
    private int _numRatingsReceived = 0;
    public FloatChannel starRatingChannel;
    
    private void OnEnable()
    {
        pickupChannel.Entered += PickupEntered;
        dropoffChannel.Entered += DropoffEntered;
        dropoffChannel.TimerExpired += DropoffTimerExpired;
    }

    private void Start()
    {
        InitRatingInfo();
        TransitionTo(startingStage);
    }

    private void InitRatingInfo()
    {
        _ratingSum = StartingRating;
        _numRatingsReceived = 1;
        UpdateStarRating();
    }

    public void RateDelivery(float rating)
    {
        _ratingSum += rating;
        _numRatingsReceived++;
        UpdateStarRating();
    } 

    public void UpdateStarRating()
    {
        _starRating = _ratingSum / _numRatingsReceived;
        starRatingChannel.ValueChanged?.Invoke(_starRating);
    }

    public void TransitionTo(GameStage gameStage)
    {
        if (currentStage)
            currentStage.OnStageExit();
        currentStage = gameStage;
        currentStage.OnStageEnter();
    }

    [CanBeNull]
    private SpawnPoint GetAvailableSpawn()
    {
        foreach (var spawnPoint in destinationLocations)
        {
            if (spawnPoint.Available)
                return spawnPoint;
        }

        return null;
    }

    [CanBeNull]
    private SpawnPoint FindMatchingSpawnPoint(Destination destination)
    {
        foreach (var spawnPoint in destinationLocations)
        {
            if (spawnPoint.areaOfInterest == destination)
            {
                return spawnPoint;
            }
        }

        return null;
    }

    [CanBeNull]
    public Destination SpawnDestinationWherePossible(PackageType packageType, bool andStartTimer = true)
    {
        var availableSpawn = GetAvailableSpawn();
        if (availableSpawn == null)
        {
            Debug.LogError("Unable to find available spawn");
            return null;
        }
        
        var destination =  SpawnDestination(availableSpawn, packageType);
        if (andStartTimer)
            destination.StartTimer(DestinationCountdownTime);
        return destination;
    }
    
    public Destination SpawnDestination(int spawnNumber, PackageType packageType, bool andStartTimer = true)
    {
        var destination = SpawnDestination(destinationLocations[spawnNumber], packageType);
        if (andStartTimer)
            destination.StartTimer(DestinationCountdownTime);
        return destination;
    }
    
    private Destination SpawnDestination(SpawnPoint spawnPoint, PackageType packageType)
    {
        
        var destination = Instantiate(destinationPrefab, spawnPoint.transform.position, Quaternion.identity);
        destination.SetPackageType(packageType);
        destination.spriteRenderer.sprite = packageType.destinationSprite;
        spawnPoint.areaOfInterest = destination;
        _destinations.Add(destination);
        return destination;
    }

    public Destination GetDestination(int index)
    {
        return _destinations[index];
    }

    public void InstantlyCreatePackage(int sourceIndex, PackageType packageType)
    {
        var source = GetSource(sourceIndex);
        source.InstantlyCreatePackage(packageType);
    }

    public void BeginCreatingPackage(int sourceIndex, PackageType packageType)
    {
        var source = GetSource(sourceIndex);
        source.BeginCreatingPackage(packageType, PackageCreationTime);
    }
    
    public Producer GetSource(int index)
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

    private int GetDeliveryRating(bool packageTypesMatch, float timeRemainingRatio)
    {
        if (!packageTypesMatch)
            return 1;

        const float baselineScore = 1;
        const float maxScore = 5;
        return (int)(maxScore * timeRemainingRatio + baselineScore);
    }

    private void DropoffEntered(AreaOfInterest area)
    {
        if (!playerInventory.HasPackage())
            return;

        var destination = area as Destination;

        destination.CancelTimer();
        var pack = playerInventory.GetPackage(0);
        bool packageTypeMatches = pack.PackageType == destination.PackageType;
        int rating = GetDeliveryRating(packageTypeMatches, destination.TimeRemainingRatio);
        RateDelivery(rating);
        
        if (packageTypeMatches)
        {
            destination.DropoffSucceeded(rating);
            IncrementSuccessfulDropoffs();
        }
        else
        {
            destination.DropoffMismatch(rating);
            IncrementIncorrectDropoffs();
        }
        playerInventory.DropoffPackage();
        FindMatchingSpawnPoint(destination)?.SetAreaOfInterest(null);
    }
    
    private void DropoffTimerExpired(AreaOfInterest area)
    {
        Debug.Log($"{area.name}'s timer expired");
        var destination = area as Destination;

        var rating = RatingForTimingMiss;
        RateDelivery(rating);
        IncrementMissedDropoffs();
        destination.DropoffMissed(rating);
        FindMatchingSpawnPoint(destination)?.SetAreaOfInterest(null);
    }

    private void IncrementSuccessfulDropoffs()
    {
        _successfulDropoffs++;
        successfulDropoffsChannel.ValueChanged?.Invoke(_successfulDropoffs);
    }

    private void IncrementMissedDropoffs()
    {
        _missedDropoffs++;
        missedDropoffsChannel.ValueChanged?.Invoke(_missedDropoffs);
    }
    
    private void IncrementIncorrectDropoffs()
    {
        _incorrectDropoffs++;
        incorrectDropoffsChannel.ValueChanged?.Invoke(_incorrectDropoffs);
    }
}
