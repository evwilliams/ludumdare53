using UnityEngine;

public class AreaOfInterest : MonoBehaviour
{
    public AOIChannel outputChannel;
    
    private void OnTriggerEnter(Collider other)
    {
        outputChannel.Entered?.Invoke(this);
    }

    private void OnTriggerExit(Collider other)
    {
        outputChannel.Exited?.Invoke(this);
    }
}
