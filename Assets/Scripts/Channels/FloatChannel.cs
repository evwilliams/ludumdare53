using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/FloatChannel")]
public class FloatChannel : ScriptableObject
{
    public UnityAction<float> ValueChanged;
}
