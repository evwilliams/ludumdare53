using System;
using TMPro;
using UnityEngine;

public class AreaOfInterest : MonoBehaviour
{
    public AOIChannel outputChannel;
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI bubbleText;
    
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

    public void DropoffSucceeded(int rating)
    {
        bubbleText.text = $"Rated {rating} stars!";
        Destroy(gameObject, 2);
    }

    public void StartTimer(float seconds)
    {
        _timer.StartTimer(seconds, OnTimerDone);
    }

    public float TimeRemainingRatio => _timer.GetRemainingRatio();

    public void CancelTimer() => _timer.Cancel();

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
