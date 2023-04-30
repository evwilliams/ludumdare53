using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsTutorialStage : GameStage
{
    public PackageType firstPackageType;
    public GameObject tutorialPanelRoot;
    
    public override void OnStageEnter()
    {
        // Debug.Log($"Entering {name}");
        tutorialPanelRoot.SetActive(true);
        gameDirector.SpawnDestination(0, firstPackageType, false);
        gameDirector.InstantlyCreatePackage(0, firstPackageType);
    }

    public override void OnStageExit()
    {
        // Debug.Log($"Exiting {name}");
        tutorialPanelRoot.SetActive(false);
    }
}
