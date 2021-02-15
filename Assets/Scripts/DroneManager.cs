using UnityEngine;

public class DroneManager : Singleton<DroneManager>
{
    protected DroneManager() {}
    
    private AudioSource m_DroneAudioSource;

    public bool isPlaying;
    
    private void Start()
    {
        m_DroneAudioSource = GetComponent<AudioSource>();
        isPlaying = false;
    }
    

    public void Toggle()
    {
        isPlaying = !isPlaying;
        if (isPlaying == false)
        {
            m_DroneAudioSource.Stop(); 
        }
        else
        {
            m_DroneAudioSource.Play();  
        }
    }

}
