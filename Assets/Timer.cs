using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float duration;
    public float startTime;
    public float doneTime;
    public bool isDone = true;
    public bool isCanceled = false;
    private Action _onTimerDone;
    
    public void StartTimer(float seconds, Action onTimerDone)
    {
        duration = seconds;
        startTime = Time.time;
        doneTime = startTime + duration;
        isDone = false;
        isCanceled = false;
        _onTimerDone = onTimerDone;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDone || isCanceled) 
            return;

        if (Time.time >= doneTime)
        {
            TimerComplete();
        }
    }

    public void Cancel()
    {
        isCanceled = true;
        _onTimerDone = null;
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
