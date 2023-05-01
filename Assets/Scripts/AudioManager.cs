using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioPlayer;
    public DestinationChannel destinationChannel;
    public ProducerChannel producerChannel;
    public AOIChannel cloudChannel;
    
    public AudioClip clip_ProducerStarted;
    public AudioClip clip_ProducerCompleted;
    public AudioClip clip_cloudCollision;
    public AudioClip clip_scoreScreen;
    
    private void OnEnable()
    {   
        destinationChannel.ResponseDisplayed += ResponseDisplayed;
        producerChannel.ProducerStarted += ProducerStarted;
        producerChannel.ProducerCompleted += ProducerCompleted;
        cloudChannel.Entered += CloudCollision;
    }

    private void CloudCollision(AreaOfInterest arg0)
    {
        audioPlayer.PlayOneShot(clip_cloudCollision);
    }

    private void OnDisable()
    {
        destinationChannel.ResponseDisplayed -= ResponseDisplayed;
        producerChannel.ProducerStarted -= ProducerStarted;
        producerChannel.ProducerCompleted -= ProducerCompleted;
    }

    private void ResponseDisplayed(Response response)
    {
        audioPlayer.PlayOneShot(response.audioClip);
    }

    private void ProducerStarted(Producer arg0)
    {
        audioPlayer.PlayOneShot(clip_ProducerStarted, 0.5f);
    }

    private void ProducerCompleted(Producer arg0)
    {
        audioPlayer.PlayOneShot(clip_ProducerCompleted, 0.5f);
    }

    public void PlayScoreScreenSound()
    {
        audioPlayer.PlayOneShot(clip_scoreScreen);
    }
}
