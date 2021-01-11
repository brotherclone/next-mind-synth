using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject dronePanel;
    private List<GameObject> _panels = new List<GameObject>();
    private Animator _currentAnimator;
 
    private void InitializeUI()
    {
        _panels.Add(dronePanel);
        Debug.Log("UI Loaded");
    }

    public void TogglePanel(PanelName panelName, TagUIInteraction _tagUIInteraction)
    {
        switch (panelName)
        {
            case PanelName.DronePanel:
                _currentAnimator = dronePanel.GetComponent<Animator>();
                var status = _currentAnimator.GetBool("isDisplayed");
                _currentAnimator.SetBool("isDisplayed", !status);
                _tagUIInteraction.EnableDisableTag(true);
                break;
        }
    }
    
    private void Start()
    {
        InitializeUI();
    }

    private void Update()
    {
        
    }
    
}

public enum UIActions{
    Open,
    Close
}

public enum UIType
{
    Panel
}

public enum PanelName
{
    None,
    DronePanel,
    PitchPanel,
    VolumePanel,
    PlayPanel
}
