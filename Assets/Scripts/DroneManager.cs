using UnityEngine;

public class DroneManager : Singleton<DroneManager>
{
    protected DroneManager() {}
    
    private AudioSource m_DroneAudioSource;

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
    
    private void Start()
    {
        m_DroneAudioSource = GetComponent<AudioSource>();
        isPlaying = false;
    }

    public void TurnDroneOnOff(bool isOn)
    {
        isPlaying = isOn;
        UIManager.Instance.UpdateDroneButtons(isOn);
        PlayStopDrone();
    }

    private void PlayStopDrone()
    {
        if (isPlaying == true)
        {
            m_DroneAudioSource.Play(); 
        }
        else
        {
            m_DroneAudioSource.Stop(); 
        }
    }
    
}
