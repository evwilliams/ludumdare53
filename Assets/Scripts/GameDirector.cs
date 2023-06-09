using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public const int DestinationCountdownTime = 9;
    public const int PackageCreationTime = 3;
    public const int StartingRating = 4;

    public const int MaxRating = 5;
    public const int RatingForTimingMiss = 0;
    
    public Inventory playerInventory;
    public List<Producer> sources = new();
    private List<Destination> _destinations = new();

    public Destination destinationPrefab;
    public List<SpawnPoint> destinationLocations = new();

    public AOIChannel pickupChannel;
    public DestinationChannel destinationChannel;
    public AOIChannel cloudChannel;
    
    public Cloud cloudPrefab;
    public Transform[] cloudSpawnPoints;
    public Package packagePrefab;

    public GameStage startingStage;
    public GameStage currentStage;
    
    private int _successfulDropoffs = 0;
    public int SuccessfulDropoffs
    {
        get => _successfulDropoffs;
    }
    
    private int _missedDropoffs = 0;
    public int MissedDropoffs
    {
        get => _missedDropoffs;
    }
    
    private int _incorrectDropoffs = 0;
    public int IncorrectDropoffs
    {
        get => _incorrectDropoffs;
    }

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
        pickupChannel.Stayed += OnStayedInProducer;
        destinationChannel.Entered += DropoffEntered;
        destinationChannel.TimerExpired += DropoffTimerExpired;
        cloudChannel.Entered += OnCloudCollision;
    }
    
    private void OnDisable()
    {
        pickupChannel.Entered -= PickupEntered;
        pickupChannel.Stayed -= OnStayedInProducer;
        destinationChannel.Entered -= DropoffEntered;
        destinationChannel.TimerExpired -= DropoffTimerExpired;
        cloudChannel.Entered -= OnCloudCollision;
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

    public int AvailableProducersCount()
    {
        int count = 0;
        foreach (var producer in sources)
        {
            if (producer.IsAvailable())
            {
                count++;
            }
        }

        return count;
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
    public Destination SpawnDestinationIfPossible(PackageType packageType, bool andStartTimer = true)
    {
        var availableSpawn = GetAvailableSpawn();
        if (availableSpawn == null)
        {
            Debug.Log("Unable to find available spawn");
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

    public void InstantlyCreatePackage(int sourceIndex, PackageType packageType, bool updateLabel = true)
    {
        var source = GetSource(sourceIndex);
        source.InstantlyCreatePackage(packageType, updateLabel);
    }

    public void BeginCreatingPackage(int sourceIndex, PackageType packageType)
    {
        BeginCreatingPackage(GetSource(sourceIndex), packageType);
    }

    public void BeginCreatingPackage(Producer producer, PackageType packageType)
    {
        producer.BeginCreatingPackage(packageType, PackageCreationTime);
    }
    
    public Producer GetSource(int index)
    {
        return sources[index];
    }

    private void PickupEntered(AreaOfInterest area)
    {
        if (!playerInventory.CanTakePackage())
            return;

        var producer = area as Producer;
        if (!producer.HasPackageReady())
            return;
        
        // Debug.Log($"Pickup entered: {area.name}");
        var package = Instantiate(packagePrefab);
        package.SetPackageType(area.PackageType);
        playerInventory.TryPickup(package);
        producer.PickupPackage();
    }
    
    private void OnStayedInProducer(AreaOfInterest area)
    {
        if (!playerInventory.CanTakePackage())
            return;

        var producer = area as Producer;
        if (!producer.HasPackageReady())
            return;
        
        // Debug.Log($"Pickup entered: {area.name}");
        var package = Instantiate(packagePrefab);
        package.SetPackageType(area.PackageType);
        playerInventory.TryPickup(package);
        producer.PickupPackage();
    }

    private int GetDeliveryRating(bool packageTypesMatch, float timeRemainingRatio)
    {
        if (!packageTypesMatch)
            return 1;
        
        if (timeRemainingRatio <= 0)
            return 0;

        if (timeRemainingRatio > 0.15)
            return MaxRating;

        return 4;
    }

    private void DropoffEntered(AreaOfInterest area)
    {
        if (!playerInventory.HasPackage())
            return;

        var destination = area as Destination;

        destination.CancelTimer();
        var pack = playerInventory.GetPackageForDropoff();
        bool packageTypeMatches = pack.PackageType == destination.PackageType;
        int rating = GetDeliveryRating(packageTypeMatches, destination.TimeRemainingRatio);
        RateDelivery(rating);
        
        if (packageTypeMatches)
        {
            destination.DropoffSucceeded(rating);
            OnSuccessfulDropoff(destination);
        }
        else
        {
            destination.DropoffMismatch(rating);
            OnIncorrectDropoff(destination);
        }
        playerInventory.DropoffPackage();
        FindMatchingSpawnPoint(destination)?.SetAreaOfInterest(null);
    }
    
    private void DropoffTimerExpired(AreaOfInterest area)
    {
        // Debug.Log($"{area.name}'s timer expired");
        var destination = area as Destination;

        var rating = RatingForTimingMiss;
        RateDelivery(rating);
        OnMissedDropoff(destination);
        destination.DropoffMissed(rating);
        FindMatchingSpawnPoint(destination)?.SetAreaOfInterest(null);
    }

    private void OnSuccessfulDropoff(Destination destination)
    {
        _successfulDropoffs++;
        destinationChannel.SuccessfulDropoff?.Invoke(destination, _successfulDropoffs);
    }

    private void OnMissedDropoff(Destination destination)
    {
        _missedDropoffs++;
        destinationChannel.MissedDropoff?.Invoke(destination, _missedDropoffs);
    }
    
    private void OnIncorrectDropoff(Destination destination)
    {
        _incorrectDropoffs++;
     
        destinationChannel.IncorrectDropoff?.Invoke(destination, _incorrectDropoffs);
    }

    Transform GetRandomCloudSpawnPoint()
    {
        return cloudSpawnPoints[Random.Range (0, cloudSpawnPoints.Length)];
    }
    
    public void SpawnCloud()
    {
        Instantiate(cloudPrefab, GetRandomCloudSpawnPoint().position, Quaternion.identity);
    }
    
    private void OnCloudCollision(AreaOfInterest arg0)
    {
        Debug.Log("Cloud collision!");
        playerInventory.MixupPackages();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
