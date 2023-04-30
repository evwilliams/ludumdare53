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

    public override void SetPackageType(PackageType packageType)
    {
        base.SetPackageType(packageType);
        spriteRenderer.color = packageType.color;
        timerFill.color = packageType.color;
    }
    
    public void InstantlyCreatePackage(PackageType packageType)
    {
        SetPackageType(packageType);
        gameObject.SetActive(true);
        timerUI.enabled = false;
        bodyAnimator.SetBool(IsProducing, false);
    }

    public void BeginCreatingPackage(PackageType packageType, float creationTime)
    {
        SetPackageType(packageType);
        gameObject.SetActive(true);
        timerUI.enabled = true;
        bodyAnimator.SetBool(IsProducing, true);
        StartTimer(creationTime);
    }

    protected override void OnTimerDone()
    {
        bodyAnimator.SetBool(IsProducing, false);
        timerUI.enabled = false;
        base.OnTimerDone();
    }
}
