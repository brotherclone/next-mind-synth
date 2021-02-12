using System;
using System.Collections;
using System.Collections.Generic;
using NextMind.Calibration;
using UnityEngine;
using UnityEngine.UI;


public class CalibrationUIManager : Singleton<CalibrationUIManager>
{
    protected CalibrationUIManager() {}
    
    public GameObject calibrationButtonGroup;
    
    public GameObject postCalibrationButtonGroup;
    
    public Text calibrationText;

    public string calibrationTextMessage;

    public int currentCalibrationTagIndex;
    
    [SerializeField]
    private List<GameObject> calibrationTagCounters;

    [SerializeField]
    private List<float> calibrationConfidence;
    
    private void Start()
    {
        ToggleState(CalibrationUIState.PreCalibration);
    }

    public void AdvanceTagCounter()
    {
        if (currentCalibrationTagIndex < 0)
        {
            currentCalibrationTagIndex = 0;
        }
        else
        {
            if (currentCalibrationTagIndex + 1 < calibrationConfidence.Count)
            {
                ++currentCalibrationTagIndex;
            } 
        }
    }

    public void TagTriggered()
    {
        calibrationConfidence[currentCalibrationTagIndex] = 1.0f;
    }

    public void UpdateConfidence(float currentConfidence)
    {
        calibrationConfidence[currentCalibrationTagIndex] = currentConfidence;
    }

    public void TagCounterAnimate()
    {
        var currentSpriteObj = calibrationTagCounters[currentCalibrationTagIndex];
        var currentSprite = currentSpriteObj.GetComponent<SpriteRenderer>();
        currentSprite.color =  new Color (0, 1, 0, calibrationConfidence[currentCalibrationTagIndex]); 
    }
    
    private void SetUpCounters()
    {
        currentCalibrationTagIndex = 0;
        foreach (var t in calibrationTagCounters)
        {
            var currentSprite = t.GetComponent<SpriteRenderer>();
            currentSprite.color =  new Color (1, 1, 1, 0.25f);
            calibrationConfidence.Add(0.0f);
        }
    }

    public void DeviceFound()
    {
        ToggleState(CalibrationUIState.Calibration);
    }
    
    private void SetUpButtons()
    {
        GameObject.Find("StartOverButton").GetComponentInChildren<Text>().text = "Start Over";
        GameObject.Find("SkipButton").GetComponentInChildren<Text>().text = "Skip Calibration";
        GameObject.Find("TryAgainButton").GetComponentInChildren<Text>().text = "Try Again";
        GameObject.Find("GetHelpButton").GetComponentInChildren<Text>().text = "Get Help";
        GameObject.Find("ProceedButton").GetComponentInChildren<Text>().text = "Proceed Anyway";
    }

    public void ToggleState(CalibrationUIState calibrationUIState)
    {
        switch (calibrationUIState)
        {
  
            case CalibrationUIState.PreCalibration:
                SetUpButtons();
                SetUpCounters();
                calibrationButtonGroup.SetActive(false);
                postCalibrationButtonGroup.SetActive(false);
                calibrationText.text = "Looking for NextMind";
                break;
            case CalibrationUIState.Calibration:
                calibrationButtonGroup.SetActive(true);
                postCalibrationButtonGroup.SetActive(false);
                calibrationText.text = "Concentrate on each tag to calibrate";
                break;
            case CalibrationUIState.PostCalibrationFailure:
                calibrationButtonGroup.SetActive(false);
                postCalibrationButtonGroup.SetActive(true);
                calibrationText.text = calibrationTextMessage;
                break;
            case CalibrationUIState.PostCalibrationSuccess:
                calibrationButtonGroup.SetActive(false);
                postCalibrationButtonGroup.SetActive(false);
                calibrationText.text = calibrationTextMessage;
                GameManager.Instance.UnLoadScene("Calibration");
                GameManager.Instance.LoadScene("NextMindSynth");
                break;
            default:
                calibrationButtonGroup.SetActive(false);
                postCalibrationButtonGroup.SetActive(false);
                calibrationText.text = "Unknown Calibration State";
                break;
        }
    }

    public void SetCalibrationText(string msg)
    {
        calibrationTextMessage = msg;
    }

    public void StartOverButtonAction()
    {
        ToggleState(CalibrationUIState.Calibration);
    }

    public void SkipButtonAction()
    {
        ToggleState(CalibrationUIState.PostCalibrationSuccess);
    }

    public void GetHelpButtonAction()
    {
        Application.OpenURL("https://www.next-mind.com/faq/");
    }

    public void ProceedButtonAction()
    {
        ToggleState(CalibrationUIState.PostCalibrationSuccess);
    }
    
}

public enum CalibrationUIState
{
    PreCalibration,
    Calibration,
    PostCalibrationFailure,
    PostCalibrationSuccess
}