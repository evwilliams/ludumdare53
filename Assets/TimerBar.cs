using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public Timer timer;
    public Slider slider;

    private void Update()
    {
        slider.enabled = !timer.isDone;
        if (!timer.isDone)
        {
            slider.value = timer.GetProgress() * slider.maxValue;
        }
    }
}
