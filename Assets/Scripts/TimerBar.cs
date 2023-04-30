using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public Timer timer;
    public Slider slider;

    private void Update()
    {
        if (timer.isDone || timer.isCanceled)
        {
            slider.gameObject.SetActive(false);
        }
        else
        {
            slider.gameObject.SetActive(true);
            slider.value = timer.GetRemainingRatio() * slider.maxValue;
        }
    }
}
