using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform[] holsters;
    private List<Package> packages = new();
    public int maxPackagesAllowed = 1;

    public bool CanTakePackage()
    {
        return packages.Count < maxPackagesAllowed;
    }
    
    public bool HasPackage()
    {
        return packages.Count > 0;
    }

    public void TryPickup(Package package)
    {
        if (CanTakePackage())
        {
            packages.Add(package);
            PositionPackages();
        }
    }

    public void PositionPackages()
    {
        for (int i = packages.Count - 1; i >= 0; i--)
        {
            var packageTransform = packages[i].transform;
            var holsterSpot = holsters[i];
            packageTransform.SetParent(holsterSpot);
            packageTransform.localPosition = Vector3.zero;
            packageTransform.localScale = holsterSpot.localScale;
        }
    }

    public void DropoffPackage()
    {
        if (!HasPackage())
            return;

        var packageToDropoff = GetPackageForDropoff();
        // Debug.Log($"Dropping of package type: {packageToDropoff.PackageType}");
        packages.Remove(packageToDropoff);
        Destroy(packageToDropoff.gameObject);
    }

    public Package GetPackageForDropoff()
    {
        return packages.First();
    }
}
