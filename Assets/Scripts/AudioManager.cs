using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioPlayer;
    public DestinationChannel destinationChannel;
    
    private void OnEnable()
    {   
        destinationChannel.ResponseDisplayed += ResponseDisplayed;
    }

    private void OnDisable()
    {
        destinationChannel.ResponseDisplayed -= ResponseDisplayed;
    }

    private void ResponseDisplayed(Response response)
    {
        audioPlayer.PlayOneShot(response.audioClip);
    }
}
