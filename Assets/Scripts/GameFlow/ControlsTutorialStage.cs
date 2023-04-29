using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsTutorialStage : GameStage
{
    public PackageType firstPackageType;
    public GameObject tutorialPanelRoot;
    
    public override void OnStageEnter()
    {
        Debug.Log($"Entering {name}");
        tutorialPanelRoot.SetActive(true);
        var destination = gameDirector.SpawnDestination(0);
        destination.SetPackageType(firstPackageType);

        var source = gameDirector.GetSource(0);
        source.SetPackageType(firstPackageType);
    }

    public override void OnStageExit()
    {
        Debug.Log($"Exiting {name}");
        tutorialPanelRoot.SetActive(false);
    }
}
