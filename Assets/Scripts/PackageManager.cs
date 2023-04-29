using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager : MonoBehaviour
{
    public Inventory playerInventory;
    public List<AreaOfInterest> pickupLocations = new();
    public List<AreaOfInterest> dropoffLocations = new();

    public AOIChannel pickupChannel;
    public AOIChannel dropoffChannel;

    public GameObject packagePrefab;
    public GameObject package;

    private void OnEnable()
    {
        pickupChannel.Entered += PickupEntered;
        dropoffChannel.Entered += DropoffEntered;
    }

    private void OnDisable()
    {
        pickupChannel.Entered -= PickupEntered;
        dropoffChannel.Entered -= DropoffEntered;
    }

    private void PickupEntered(AreaOfInterest area)
    {
        Debug.Log($"Pickup entered: {area.name}");
        if (package == null)
        {
            package = Instantiate(packagePrefab);
        }
        playerInventory.TryPickup(package.transform);
    }

    private void DropoffEntered(AreaOfInterest area)
    {
        Debug.Log($"Dropoff entered: {area.name}");
        playerInventory.TryDropoff();   
    }
}
