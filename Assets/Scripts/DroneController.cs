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
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
