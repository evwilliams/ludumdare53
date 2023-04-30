using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ScoreScreenStage : GameStage
{
    public Stage_04 stage04;
    public int endGameAfterNumDeliveriesInStage04;
    
    public GameObject scorePanelRoot;

    // TODO - Make this way of checking transitions not suck
    private void Update()
    {
        if (gameDirector.currentStage == stage04 &&
            stage04.numDeliveriesInThisStage >= endGameAfterNumDeliveriesInStage04)
        {
            gameDirector.TransitionTo(this);
        }
    }

    public override void OnStageEnter()
    {
        Debug.Log($"Entering {name}");
        scorePanelRoot.SetActive(true);
    }

    public override void OnStageExit()
    {
        Debug.Log($"Exiting {name}");
        enabled = false;
        gameObject.SetActive(false);
    }
}
