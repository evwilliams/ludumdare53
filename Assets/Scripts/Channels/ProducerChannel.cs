using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/ProducerChannel")]
public class ProducerChannel : AOIChannel
{
    public UnityAction<Producer> ProducerStarted;
    public UnityAction<Producer> ProducerCompleted;
    public UnityAction<Producer> ProducerBecameAvailable;
}
