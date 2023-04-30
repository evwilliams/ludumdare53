using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ScoreScreenStage : GameStage
{
    public Stage_04 stage04;
    public int endGameAfterNumDeliveriesInStage04;
    
    public GameObject scorePanelRoot;
    public TextMeshProUGUI scoreText_storkRating;
    public TextMeshProUGUI scoreText_successfulDeliveries;
    public TextMeshProUGUI scoreText_incorrectDeliveries;
    public TextMeshProUGUI scoreText_missedDeliveries;
    
    // So hacky
    public GameObject remainingDeliveriesUIContainer;
    public TextMeshProUGUI remainingDeliveriesCounterText;

    // TODO - Make this way of checking transitions not suck
    private void Update()
    {
        if (gameDirector.currentStage == stage04)
        {
            UpdateDeliveryCountdown();
            
            if(stage04.numDeliveriesInThisStage >= endGameAfterNumDeliveriesInStage04)
            {
                gameDirector.TransitionTo(this);
            }
        }
            
    }

    void UpdateDeliveryCountdown()
    {
        remainingDeliveriesUIContainer.SetActive(true);
        remainingDeliveriesCounterText.text =
            $"{stage04.numDeliveriesInThisStage}/{endGameAfterNumDeliveriesInStage04}";
    }

    void UpdateScores()
    {
        scoreText_storkRating.text = gameDirector.StarRating.ToString("0.00");
        scoreText_successfulDeliveries.text = gameDirector.SuccessfulDropoffs.ToString();
        scoreText_incorrectDeliveries.text = gameDirector.IncorrectDropoffs.ToString();
        scoreText_missedDeliveries.text = gameDirector.MissedDropoffs.ToString();
    }

    public override void OnStageEnter()
    {
        Debug.Log($"Entering {name}");
        remainingDeliveriesUIContainer.SetActive(false);
        UpdateScores();
        scorePanelRoot.SetActive(true);
    }

    public override void OnStageExit()
    {
        Debug.Log($"Exiting {name}");
        enabled = false;
        gameObject.SetActive(false);
    }
}
