using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Producer : AreaOfInterest
{
    public Animator bodyAnimator;
    public Image sliderFill;
    private static readonly int IsProducing = Animator.StringToHash("isProducing");

    public override void SetPackageType(PackageType packageType)
    {
        base.SetPackageType(packageType);
        spriteRenderer.color = packageType.color;
        sliderFill.color = packageType.color;
    }
    
    public void InstantlyCreatePackage(PackageType packageType)
    {
        SetPackageType(packageType);
        gameObject.SetActive(true);
        bodyAnimator.SetBool(IsProducing, false);
    }

    public void BeginCreatingPackage(PackageType packageType, float creationTime)
    {
        SetPackageType(packageType);
        gameObject.SetActive(true);
        bodyAnimator.SetBool(IsProducing, true);
        StartTimer(creationTime);
    }

    protected override void OnTimerDone()
    {
        bodyAnimator.SetBool(IsProducing, false);
        base.OnTimerDone();
    }
}
