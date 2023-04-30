using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatUIUpdater : MonoBehaviour
{
    public FloatChannel channelToMonitor;
    public TextMeshProUGUI textToUpdate;

    private void OnEnable()
    {
        channelToMonitor.ValueChanged += ValueChanged;
    }

    private void OnDisable()
    {
        channelToMonitor.ValueChanged -= ValueChanged;
    }

    private void ValueChanged(float newValue)
    {
        if (textToUpdate)
           textToUpdate.text = newValue.ToString("0.00");
    }
}
