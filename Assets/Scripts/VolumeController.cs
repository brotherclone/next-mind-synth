using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        UpdateVolume();
    }

    public void HandleSlider(Slider slider)
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        NoteManager.Instance.SetVolume(slider.value);
    }
}