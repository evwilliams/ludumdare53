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
    
    
    public IntChannel successfulDropoffsChannel;
    public IntChannel missedDropoffsChannel;
    public IntChannel incorrectDropoffsChannel;
    
    private void OnEnable()
    {
        successfulDropoffsChannel.ValueChanged += OnSuccessfulDropoff;
        missedDropoffsChannel.ValueChanged += OnMissedDropoff;
        incorrectDropoffsChannel.ValueChanged += OnIncorrectDropoff;
    }

    private void OnDisable()
    {
        successfulDropoffsChannel.ValueChanged -= OnSuccessfulDropoff;
    }

    private void OnSuccessfulDropoff(int arg0)
    {
        audioPlayer.PlayOneShot(sound_SuccessfulPickup);
    }

    private void OnIncorrectDropoff(int arg0)
    {
        audioPlayer.PlayOneShot(sound_IncorrectPickup);
    }

    private void OnMissedDropoff(int arg0)
    {
        audioPlayer.PlayOneShot(sound_MissedPickup);
    }
}
