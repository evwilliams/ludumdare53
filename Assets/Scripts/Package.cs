using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private PackageType _packageType;
    public PackageType PackageType
    {
        get => _packageType;
        set => SetPackageType(value);
    }

    public void SetPackageType(PackageType pType)
    {
        _packageType = pType;
        spriteRenderer.color = pType.color;
    }
}
