using UnityEngine;

public class MultiTutorialStage : GameStage
{
    public DestinationChannel destinationChannel;
    public GameObject multiTutorialPanelRoot;
    
    // This stage is activated if the player successfully completes a dropoff in Stage_02
    private void SuccessfulDropoffCountChanged(AreaOfInterest arg0, int successCount)
    {
        if (canTransitionFrom.Contains(gameDirector.currentStage) && successCount > 1)
            gameDirector.TransitionTo(this);
    }
    
    private void OnEnable()
    {
        destinationChannel.SuccessfulDropoff += SuccessfulDropoffCountChanged;
    }

    private void OnDisable()
    {
        destinationChannel.SuccessfulDropoff -= SuccessfulDropoffCountChanged;
    }
    
    public override void OnStageEnter()
    {
        // Debug.Log($"Entering {name}");
        multiTutorialPanelRoot.SetActive(true);
    }

    public override void OnStageExit()
    {
        // Debug.Log($"Exiting {name}");
        Destroy(multiTutorialPanelRoot, 2);
        enabled = false;
        gameObject.SetActive(false);
    }
}
