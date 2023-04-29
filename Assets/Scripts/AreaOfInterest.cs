using UnityEngine;

public class AreaOfInterest : MonoBehaviour
{
    public AOIChannel outputChannel;
    public SpriteRenderer spriteRenderer;
    public Timer timer;
    
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

    private void OnTriggerEnter(Collider other)
    {
        outputChannel.Entered?.Invoke(this);
    }

    private void OnTriggerExit(Collider other)
    {
        outputChannel.Exited?.Invoke(this);
    }
}
