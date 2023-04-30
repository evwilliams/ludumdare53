using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/IntChannel")]
public class IntChannel : ScriptableObject
{
    public UnityAction<int> ValueChanged;
}