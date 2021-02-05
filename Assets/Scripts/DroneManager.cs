using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : Singleton<DroneManager>
{
    protected DroneManager() {}
    
    private AudioSource m_DroneAudioSource;

    public bool isPlaying;

    public bool isToggled;
    
    private void Start()
    {
        m_DroneAudioSource = GetComponent<AudioSource>();
        isPlaying = false;
        isToggled = false;
    }

    private void Update()
    {
        if (isPlaying == true && isToggled == true)
        {
            m_DroneAudioSource.Play();
            isToggled = false;
        }

        if (isPlaying == false && isToggled == true)
        {
            m_DroneAudioSource.Stop();
            isToggled = false;
        }
    }   
}
