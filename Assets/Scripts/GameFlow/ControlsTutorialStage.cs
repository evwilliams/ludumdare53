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
        gameDirector.BeginCreatingPackage(0, firstPackageType);
    }

    public override void OnStageExit()
    {
        // Debug.Log($"Exiting {name}");
        tutorialPanelRoot.SetActive(false);
        enabled = false;
        gameObject.SetActive(false);
    }
}
