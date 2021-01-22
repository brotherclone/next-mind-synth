using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject dronePanel;
    public GameObject pitchPanel;
    public GameObject playPanel;
    public GameObject volumePanel;

    public TMP_Text confidenceText;
    public TMP_Text triggeringText;
    public TMP_Text midiConnectionText;
    public TMP_Text currentKeyText;
    public TMP_Text currentModeText;
    public TMP_Text currentDronesText;
    public TMP_Text currentVolumeText;
    public TMP_Text instructionsText;
    public TMP_Text alertText;
    
    private List<GameObject> _panels = new List<GameObject>();
    private List<TMP_Text> _uiTexts = new List<TMP_Text>();
    
    private Animator _currentAnimator;
 
    private void InitializeUI()
    {
        _panels.Add(dronePanel);
        _panels.Add(pitchPanel);
        _panels.Add(playPanel);
        _panels.Add(volumePanel);
        
        _uiTexts.Add(confidenceText);
        _uiTexts.Add(triggeringText);
        _uiTexts.Add(midiConnectionText);
        _uiTexts.Add(currentKeyText);
        _uiTexts.Add(currentModeText);
        _uiTexts.Add(currentDronesText);
        _uiTexts.Add(currentVolumeText);
        _uiTexts.Add(instructionsText);
        _uiTexts.Add(alertText);
        
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
