using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class AOIChannel : ScriptableObject
{
    public UnityAction<AreaOfInterest> Entered;
    public UnityAction<AreaOfInterest> Exited;
}
