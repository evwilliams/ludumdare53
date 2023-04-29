using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float duration;
    public float startTime;
    public float doneTime;
    public bool isDone = true;
    private Action _onTimerDone;
    
    public void StartTimer(float seconds, Action onTimerDone)
    {
        duration = seconds;
        startTime = Time.time;
        doneTime = startTime + duration;
        isDone = false;
        _onTimerDone = onTimerDone;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDone) 
            return;

        if (Time.time >= doneTime)
        {
            TimerComplete();
        }
    }

    public float GetTimeElapsed()
    {
        return Time.time - startTime;
    }

    public float GetTimeRemaining()
    {
        return duration - GetTimeElapsed();
    }

    public float GetRemainingRatio()
    {
        return GetTimeRemaining() / duration;
    }

    public float GetProgress()
    {
        return GetTimeElapsed() / duration;
    }

    void TimerComplete()
    {
        isDone = true;
        _onTimerDone?.Invoke();
    }
}
