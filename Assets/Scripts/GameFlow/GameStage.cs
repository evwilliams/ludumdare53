using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStage : MonoBehaviour
{
    public GameDirector gameDirector;
    public List<GameStage> canTransitionFrom;

    public abstract void OnStageEnter();
    public abstract void OnStageExit();
}
