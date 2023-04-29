using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PackageManager : MonoBehaviour
{
    private const int DestinationCountdownTime = 9;
    public Inventory playerInventory;
    public List<AreaOfInterest> sources = new();
    public List<AreaOfInterest> destinations = new();

    public AOIChannel pickupChannel;
    public AOIChannel dropoffChannel;

    public Package packagePrefab;

    public PackageType[] packageTypes;
    
    private void OnEnable()
    {
        pickupChannel.Entered += PickupEntered;
        dropoffChannel.Entered += DropoffEntered;
        dropoffChannel.TimerExpired += DropoffTimerExpired;
    }

    private void DropoffTimerExpired(AreaOfInterest destination)
    {
        Debug.Log($"{destination.name}'s timer expired");
        AssignNewType(destination);
    }

    private void Start()
    {
        foreach (var source in sources)
        {
            source.SetPackageType(packageTypes[0]);
        }

        foreach (var destination in destinations)
        {
            AssignNewType(destination);
        }
    }

    void AssignNewType(AreaOfInterest area)
    {
        area.SetPackageType(GetRandomPackageType());
        area.StartTimer(DestinationCountdownTime);
    }

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
        
        Debug.Log($"Pickup entered: {area.name}");
        var package = Instantiate(packagePrefab);
        package.SetPackageType(area.PackageType);
        playerInventory.TryPickup(package);
    }

    private void DropoffEntered(AreaOfInterest area)
    {
        if (!playerInventory.HasPackage())
            return;
        
        Debug.Log($"Dropoff entered: {area.name}");
        var pack = playerInventory.GetPackage(0);
        if (pack.PackageType == area.PackageType)
            playerInventory.TryDropoff();
    }
}
