using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneController : MonoBehaviour
{
    public Toggle toggle;

    public void HandleToggle(Toggle toggle)
    {
        Debug.Log(" t -->"+ toggle.isOn);
        ChangeDrone();
    }


    public void ChangeDrone()
    {
        DroneManager.Instance.Toggle();
    }
}
