using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioPlayer;

    public AudioClip sound_SuccessfulPickup;
    public AudioClip sound_MissedPickup;
    public AudioClip sound_IncorrectPickup;


    public DestinationChannel destinationChannel;
    
    private void OnEnable()
    {
        destinationChannel.SuccessfulDropoff += OnSuccessfulDropoff;
        destinationChannel.MissedDropoff += OnMissedDropoff;
        destinationChannel.IncorrectDropoff += OnIncorrectDropoff;
    }

    private void OnDisable()
    {
        destinationChannel.SuccessfulDropoff -= OnSuccessfulDropoff;
        destinationChannel.MissedDropoff -= OnMissedDropoff;
        destinationChannel.IncorrectDropoff -= OnIncorrectDropoff;
    }

    private void OnSuccessfulDropoff(Destination destination, int i)
    {
        audioPlayer.PlayOneShot(sound_SuccessfulPickup);
    }

    private void OnMissedDropoff(Destination destination, int i)
    {
        audioPlayer.PlayOneShot(sound_MissedPickup);
    }

    private void OnIncorrectDropoff(Destination destination, int i)
    {
        audioPlayer.PlayOneShot(sound_IncorrectPickup);
    }
}
