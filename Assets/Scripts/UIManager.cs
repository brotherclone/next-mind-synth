using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.EditorUtilities;

public class UIManager : Singleton<UIManager>
{
    
    public TMP_Text confidenceText;
    public TMP_Text triggeringText;
    public TMP_Text midiConnectionText;
    public TMP_Text currentKeyText;
    public TMP_Text currentModeText;
    public TMP_Text currentDronesText;
    public TMP_Text currentVolumeText;
    public TMP_Text instructionsText;
    public TMP_Text alertText;
    
    private List<TMP_Text> _uiTexts = new List<TMP_Text>();
    
    private void InitializeUI()
    {
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
    

    private void InitializeInfoTexts()
    {
        foreach (var text in _uiTexts)
        {
            text.text = "";
        }
    }
    
    public void UpDateInfoTexts(InfoText infoText, string message)
    {
        switch (infoText)
        {
            case InfoText.Volume:
                if (message == "!MUTE!")
                {
                    currentVolumeText.text = "Mute";
                }
                else
                { 
                    currentVolumeText.text = "Volume: " + message;
                }
                break;
            case InfoText.CurrentKey:
                currentKeyText.text =  "Key: " + message;
                break;
            default:
                alertText.text = "";
                break;
        }
    }
    
    private void Start()
    {
        InitializeUI();
        InitializeInfoTexts();
    }
    
}


public enum InfoText
{
    Confidence,
    Triggering,
    MIDIConnection,
    CurrentKey,
    CurrentMode,
    CurrentDrones,
    Volume,
    Instructions,
    Alerts
}