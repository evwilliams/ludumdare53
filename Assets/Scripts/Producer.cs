using UnityEngine;
using UnityEngine.UI;

public class Producer : AreaOfInterest
{
    public ProducerChannel outputChannel;
    
    public Animator bodyAnimator;
    public Slider timerUI;
    public Image timerFill;
    private static readonly int IsProducing = Animator.StringToHash("isProducing");
    private bool _packageReady = false;

    public override void SetPackageType(PackageType packageType)
    {
        base.SetPackageType(packageType);
        spriteRenderer.color = packageType.color;
        timerFill.color = packageType.color;
    }

    public bool IsAvailable()
    {
        return !GetIsProducing() && !HasPackageReady();
    }

    public bool GetIsProducing()
    {
        return bodyAnimator.GetBool(IsProducing);
    }
    
    public bool HasPackageReady()
    {
        return _packageReady;
    }

    public void InstantlyCreatePackage(PackageType packageType, bool updateLabel = true)
    {
        SetPackageType(packageType);
        gameObject.SetActive(true);
        OnPackageReady(updateLabel);
    }

    public void BeginCreatingPackage(PackageType packageType, float creationTime)
    {
        SetPackageType(packageType);
        gameObject.SetActive(true);
        timerUI.enabled = true;

        OnStartCreating();
        StartTimer(creationTime);
    }

    void OnStartCreating()
    {
        _packageReady = false;
        bubbleText.text = "Baby Making";
        bodyAnimator.SetBool(IsProducing, true);
        outputChannel.ProducerStarted?.Invoke(this);
    }

    void OnPackageReady(bool updateLabel = true)
    {
        _packageReady = true;
        if (updateLabel)
            bubbleText.text = "Baby Ready!";
        bodyAnimator.SetBool(IsProducing, false);
        timerUI.enabled = false;
        outputChannel.ProducerCompleted?.Invoke(this);
    }

    public override AOIChannel GetOutputChannel()
    {
        return outputChannel;
    }

    protected override void OnTimerDone()
    {
        OnPackageReady();
        base.OnTimerDone();
    }

    public void PickupPackage()
    {
        _packageReady = false;
        bubbleText.text = "Baby Printer";
        outputChannel.ProducerBecameAvailable?.Invoke(this);
    }
}
