using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/DestinationChannel")]
public class DestinationChannel : AOIChannel
{
    public UnityAction<Destination, int> SuccessfulDropoff;
    public UnityAction<Destination, int> MissedDropoff;
    public UnityAction<Destination, int> IncorrectDropoff;
    
    public UnityAction<Response> ResponseDisplayed;
}
