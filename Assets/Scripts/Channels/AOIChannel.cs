using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/AreaOfInterest")]
public class AOIChannel : ScriptableObject
{
    public UnityAction<AreaOfInterest> Entered;
    public UnityAction<AreaOfInterest> Exited;
    public UnityAction<AreaOfInterest> TimerExpired;
}
