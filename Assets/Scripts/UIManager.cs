using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    protected UIManager() {}
    
    public List<Text> noteTexts;
    public Button waveTriangle;
    public Button waveSquare;
    public Button waveSawtooth;
    public Button oscOn;
    public Button oscOff;

    public void RefreshNoteTexts()
    {
        for (var i = 0; i < NoteManager.Instance.currentScale.Count; ++i)
        {
            noteTexts[i].text = NoteManager.Instance.currentScale[i].note_name;
        }
    }

    public void UpdateWaveButtons(SignalType signalType, bool isInitial)
    {
        // This is fucking stupid.
        ButtonToggle(waveSawtooth, false);
        ButtonToggle(waveSquare, false);
        ButtonToggle(waveTriangle, false);

        switch (signalType)
        {
            case SignalType.Sawtooth:
                ButtonToggle(waveSawtooth, isInitial);
                break;
            case SignalType.Square:
                ButtonToggle( waveSquare, isInitial);
                break;
            case SignalType.Triangle:
                ButtonToggle(waveTriangle, isInitial);
                break;
            default:
                Debug.Log("Unknown Signal Type for button change.");
                break;
        }
    }


    public void UpdateOSCButtons(bool isOn)
    {
        ButtonToggle(oscOff, false);
        ButtonToggle(oscOn, false);
        if (isOn)
        {
            ButtonToggle(oscOn, false);
            ToggleButtonText(oscOn, true);
            ToggleButtonText(oscOff, false);
        }
        else
        {
            ButtonToggle(oscOff, false);
            ToggleButtonText(oscOn, false);
            ToggleButtonText(oscOff, true);
        }
    }

    private void ButtonToggle(Button button, bool isInitial)
    {
        if (isInitial == false)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.pressedColor = Color.magenta;
            colors.highlightedColor = Color.cyan;
            colors.selectedColor = Color.cyan;
            button.colors = colors;
        }
        else
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.cyan;
            colors.pressedColor = Color.cyan;
            colors.highlightedColor = Color.cyan;
            colors.selectedColor = Color.cyan;
            button.colors = colors;
        }
    }
    
    private void ToggleButtonText(Button button, bool isOn)
    {
        var text = button.GetComponentInChildren<Text>();
        if (isOn)
        {
            text.color = Color.cyan;
        }
        else
        {
            text.color = Color.white;
        }
    }
    
    private void InitializeUI()
    {
        Debug.Log("UI Loaded");
        ButtonToggle(oscOn, true);
        ToggleButtonText(oscOn, true);
    }
    
    private void Start()
    {
        InitializeUI();
    }
    
    public void HandleKeyRootDropdown(int selection)
    {
        NoteManager.Instance.SetCurrentScaleRoot(69+selection);
    }

    public void HandleKeyModeDropdown(int selection)
    {
        
        var index = 0;
        
        foreach (ScalesAndModes scaleAndMode in Enum.GetValues(typeof(ScalesAndModes)))
        {
            if (index == selection)
            {
               NoteManager.Instance.SetCurrentScaleMode(scaleAndMode);
            }
            ++index;
        }
    }
}
