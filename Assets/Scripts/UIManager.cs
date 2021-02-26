using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    protected UIManager()
    {
    }

    public List<Text> noteTexts;
    public Button waveTriangle;
    public Button waveSquare;
    public Button waveSawtooth;
    public Button oscOn;
    public Button oscOff;
    public Button droneOn;
    public Button droneOff;

    public Color salmon = new Color(255, 92, 92, 1);
    public Color seaMist = new Color(99, 185, 149, 1);
    public Color deepRed = new Color(19, 0, 0, 1);
    public Color offBlack = new Color(19, 0, 0, 1);
    public Color foggyDay = new Color(125, 132, 145, 1);
    public Color offWhite = new Color(250, 255, 255, 1);

    public void RefreshNoteTexts()
    {
        for (var i = 0; i < NoteManager.Instance.currentScale.Count; ++i)
        {
            noteTexts[i].text = NoteManager.Instance.currentScale[i].note_name;
        }
    }

    public void UpdateWaveButtons(SignalTypes signalTypes, bool isInitial)
    {
        // This is fucking stupid.
        ButtonToggle(waveSawtooth, false);
        ButtonToggle(waveSquare, false);
        ButtonToggle(waveTriangle, false);

        switch (signalTypes)
        {
            case SignalTypes.Sawtooth:
                ButtonToggle(waveSawtooth, isInitial);
                break;
            case SignalTypes.Square:
                ButtonToggle(waveSquare, isInitial);
                break;
            case SignalTypes.Triangle:
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

    public void UpdateDroneButtons(bool isOn)
    {
        ButtonToggle(droneOff, false);
        ButtonToggle(droneOn, false);
        if (isOn)
        {
            ButtonToggle(droneOn, false);
            ToggleButtonText(droneOn, true);
            ToggleButtonText(droneOff, false);
        }
        else
        {
            ButtonToggle(droneOff, false);
            ToggleButtonText(droneOn, false);
            ToggleButtonText(droneOff, true);
        }
    }


    private void ButtonToggle(Button button, bool isInitial)
    {
        if (isInitial == false)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = offWhite;
            colors.pressedColor = salmon;
            colors.highlightedColor = seaMist;
            colors.selectedColor = seaMist;
            button.colors = colors;
        }
        else
        {
            ColorBlock colors = button.colors;
            colors.normalColor = seaMist;
            colors.pressedColor = seaMist;
            colors.highlightedColor = seaMist;
            colors.selectedColor = seaMist;
            button.colors = colors;
        }
    }

    private void ToggleButtonText(Button button, bool isOn)
    {
        var text = button.GetComponentInChildren<Text>();
        if (isOn)
        {
            text.color = seaMist;
        }
        else
        {
            text.color = offWhite;
        }
    }

    private void InitializeUI()
    {
        Debug.Log("UI Loaded");
        ButtonToggle(oscOn, true);
        ToggleButtonText(oscOn, true);
        ButtonToggle(droneOn, true);
        ToggleButtonText(droneOn, true);
    }

    private void Start()
    {
        InitializeUI();
    }

    public void HandleKeyRootDropdown(int selection)
    {
        NoteManager.Instance.SetCurrentScaleRoot(69 + selection);
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