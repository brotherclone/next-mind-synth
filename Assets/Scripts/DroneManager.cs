﻿using UnityEngine;

public class DroneManager : Singleton<DroneManager>
{
    protected DroneManager() {}
    
    private AudioSource _mDroneAudioSource;

    public bool isPlaying;

    public AudioClip cDrone;
    public AudioClip cSharpDrone;
    public AudioClip dDrone;
    public AudioClip dSharpDrone;
    public AudioClip eDrone;
    public AudioClip fDrone;
    public AudioClip fSharpDrone;
    public AudioClip gDrone;
    public AudioClip gSharpDrone;
    public AudioClip aDrone;
    public AudioClip aSharpDrone;
    public AudioClip bDrone;

    private AudioClip _currentDrone;
    
    private void Start()
    {
        _mDroneAudioSource = GetComponent<AudioSource>();
        isPlaying = false;
    }

    public void SetDroneNote(Note note)
    {
        _mDroneAudioSource.Stop(); 
        switch (note.note_name)
        {
            case "A":
                _currentDrone = aDrone;
                break;
            case "A#":
                _currentDrone = aSharpDrone;
                break;
            case "B":
                _currentDrone = bDrone;
                break;
            case "C":
                _currentDrone = cDrone;
                break;
            case "C#":
                _currentDrone = cSharpDrone;
                break;
            case "D":
                _currentDrone = dDrone;
                break;
            case "D#":
                _currentDrone = dSharpDrone;
                break;
            case "E":
                _currentDrone = eDrone;
                break;
            case "F":
                _currentDrone = fDrone;
                break;
            case "F#":
                _currentDrone = fSharpDrone;
                break;
            case "G":
                _currentDrone = gDrone;
                break;
            case "G#":
                _currentDrone = gSharpDrone;
                break;
            default:
                Debug.Log("Unknown note sent to Drone Manager. Setting to 440/A anyway.");
                _currentDrone = aDrone;
                break;
        }
        _mDroneAudioSource.clip = _currentDrone;
        _mDroneAudioSource.Play();
    }
    

    public void TurnDroneOnOff(bool isOn)
    {
        isPlaying = isOn;
        UIManager.Instance.UpdateDroneButtons(isOn);
        PlayStopDrone();
    }

    private void PlayStopDrone()
    {
        if (isPlaying)
        {
            _mDroneAudioSource.Play(); 
        }
        else
        {
            _mDroneAudioSource.Stop(); 
        }
    }

    public void SetDroneVolume()
    {
        _mDroneAudioSource.volume = NoteManager.Instance.currentVolume;
    }
}
