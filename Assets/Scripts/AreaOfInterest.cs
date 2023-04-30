using System;
using TMPro;
using UnityEngine;

public abstract class AreaOfInterest : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI bubbleText;

    protected bool _pendingDestroy = false;
    
    [SerializeField]
    private Timer _timer;
    private PackageType _packageType;
    public PackageType PackageType
    {
        get => _packageType;
        set => SetPackageType(value);
    }

    public virtual void SetPackageType(PackageType pType)
    {
        _packageType = pType;
    }
    
    public void StartTimer(float seconds)
    {
        _timer.StartTimer(seconds, OnTimerDone);
    }

    public float TimeRemainingRatio => _timer.GetRemainingRatio();

    public void CancelTimer() => _timer.Cancel();

    public abstract AOIChannel GetOutputChannel();

    protected virtual void OnTimerDone()
    {
        GetOutputChannel().TimerExpired?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_pendingDestroy)
            return;
        
        GetOutputChannel().Entered?.Invoke(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_pendingDestroy)
            return;
        
        GetOutputChannel().Stayed?.Invoke(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_pendingDestroy)
            return;
        
        GetOutputChannel().Exited?.Invoke(this);
    }
}
