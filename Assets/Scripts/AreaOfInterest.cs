using System;
using TMPro;
using UnityEngine;

public class AreaOfInterest : MonoBehaviour
{
    public AOIChannel outputChannel;
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI bubbleText;

    private bool _pendingDestroy = false;
    
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

    public void DropoffSucceeded(int rating)
    {
        _pendingDestroy = true;
        bubbleText.text = $"Rated {rating} {StarTextForRating(rating)}!";
        Destroy(gameObject, 2);
    }
    
    public void DropoffMismatch(int rating)
    {
        _pendingDestroy = true;
        bubbleText.text = $"That's not my baby! {rating} {StarTextForRating(rating)}!";
        Destroy(gameObject, 2);
    }
    
    public void DropoffMissed(int rating)
    {
        _pendingDestroy = true;
        bubbleText.text = $"I'm taking my stork business elsewhere! {rating} {StarTextForRating(rating)}!";
        Destroy(gameObject, 2);
    }

    private String StarTextForRating(int rating)
    {
        return rating == 1 ? "star" : "stars";
    }

    public void StartTimer(float seconds)
    {
        _timer.StartTimer(seconds, OnTimerDone);
    }

    public float TimeRemainingRatio => _timer.GetRemainingRatio();

    public void CancelTimer() => _timer.Cancel();

    protected virtual void OnTimerDone()
    {
        outputChannel.TimerExpired?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_pendingDestroy)
            return;
        
        outputChannel.Entered?.Invoke(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_pendingDestroy)
            return;
        
        outputChannel.Exited?.Invoke(this);
    }
}
