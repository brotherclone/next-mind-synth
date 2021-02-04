using System;
using System.Collections;
using System.Collections.Generic;
using NextMind.Calibration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class CalibrationUIManager : Singleton<CalibrationUIManager>
{
    protected CalibrationUIManager() {}
    
    public GameObject calibrationButtonGroup;
    
    public GameObject postCalibrationButtonGroup;
    
    public TMP_Text calibrationText;

    public string calibrationTextMessage;

    public int currentCalibrationTagIndex;

    [SerializeField]
    private List<GameObject> calibrationTagCounters;
    
    
    private void Start()
    {
        ToggleState(CalibrationUIState.PreCalibration);
    }

    public void AdvanceTagCounter()
    {
        ++currentCalibrationTagIndex;
    }

    public void TagCounterAnimate()
    {
        var currentSpriteObj = calibrationTagCounters[currentCalibrationTagIndex];
        var currentSprite = currentSpriteObj.GetComponent<SpriteRenderer>();
        currentSprite.color =  new Color (0, 1, 0, 1); 
    }
    
    private void SetUpCounters()
    {
        currentCalibrationTagIndex = 0;
        foreach (var t in calibrationTagCounters)
        {
            var currentSprite = t.GetComponent<SpriteRenderer>();
            currentSprite.color =  new Color (1, 1, 1, 1); 
        }
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
            case CalibrationUIState.Calibration:
                Debug.Log("calibration");
                calibrationButtonGroup.SetActive(true);
                postCalibrationButtonGroup.SetActive(false);
                calibrationText.text = "Concentrate on each tag to calibrate";
                break;
            case CalibrationUIState.PreCalibration:
                Debug.Log("pre");
                SetUpButtons();
                SetUpCounters();
                calibrationButtonGroup.SetActive(false);
                postCalibrationButtonGroup.SetActive(false);
                calibrationText.text = "Beginning Calibration";
                break;
            case CalibrationUIState.PostCalibrationFailure:
                calibrationButtonGroup.SetActive(false);
                postCalibrationButtonGroup.SetActive(true);
                Debug.Log("failed");
                calibrationText.text = calibrationTextMessage;
                break;
            case CalibrationUIState.PostCalibrationSuccess:
                calibrationButtonGroup.SetActive(false);
                postCalibrationButtonGroup.SetActive(false);
                Debug.Log("success");
                calibrationText.text = calibrationTextMessage;
                GameManager.Instance.UnLoadScene("Calibration");
                GameManager.Instance.LoadScene("Synth");
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
        Debug.Log("StartOverButtonAction");
        ToggleState(CalibrationUIState.Calibration);
    }

    public void SkipButtonAction()
    {
        Debug.Log("SkipButtonAction");
        ToggleState(CalibrationUIState.PostCalibrationSuccess);
    }

    public void GetHelpButtonAction()
    {
        Debug.Log("GetHelpButtonAction");
    }

    public void ProceedButtonAction()
    {
        Debug.Log("ProceedButtonAction"); 
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