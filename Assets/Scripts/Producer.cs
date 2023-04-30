using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Producer : AreaOfInterest
{
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
    }

    void OnPackageReady(bool updateLabel = true)
    {
        _packageReady = true;
        if (updateLabel)
            bubbleText.text = "Baby Ready!";
        bodyAnimator.SetBool(IsProducing, false);
        timerUI.enabled = false;
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
    }
}
