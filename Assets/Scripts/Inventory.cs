using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform holster;
    private List<Package> packages = new();
    
    public bool HasPackage()
    {
        return holster.childCount > 0;
    }

    public void TryPickup(Package package)
    {
        if (!HasPackage())
        {
            var packageTransform = package.transform;
            packageTransform.position = holster.transform.position;
            packageTransform.parent = holster;
            packages.Add(package);
        }
    }

    public void TryDropoff()
    {
        if (!HasPackage())
            return;
        
        for (int i = holster.childCount - 1; i >= 0; i--)
        {
            Debug.Log($"Dropping off: {holster.GetChild(i).gameObject.name}");
            Destroy(holster.GetChild(i).gameObject);
        }
        packages.Clear();
    }

    public Package GetPackage(int index)
    {
        return packages[index];
    }
}
