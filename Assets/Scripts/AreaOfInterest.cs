using System;
using UnityEngine;

public class AreaOfInterest : MonoBehaviour
{
    public AOIChannel outputChannel;
    public SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private Timer _timer;
    private PackageType _packageType;
    public PackageType PackageType
    {
        get => _packageType;
        set => SetPackageType(value);
    }

    public void SetPackageType(PackageType pType)
    {
        _packageType = pType;
        // spriteRenderer.color = pType.color;
        spriteRenderer.sprite = pType.sprite;
    }

    public void StartTimer(float seconds)
    {
        _timer.StartTimer(seconds, OnTimerDone);
    }

    private void OnTimerDone()
    {
        outputChannel.TimerExpired?.Invoke(this);
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
