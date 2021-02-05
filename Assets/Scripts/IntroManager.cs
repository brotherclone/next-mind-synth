using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroManager : Singleton<IntroManager>
{
    protected IntroManager() {}

    [SerializeField] private List<GameObject> introSteps;

    [SerializeField] private GameObject introButtonGroup;

    [SerializeField] private TMP_Text introText;

    private int currentStep;

    private void Start()
    {
        currentStep = 0;
        SetUpButtons();
        SetUpSteps();
        DoStep();
    }

    public void AdvanceStep()
    {
        if (currentStep + 1 < introSteps.Count)
        {
            ++currentStep;
            DoStep();
        }
    }

    public void RewindStep()
    {
        if (currentStep - 1 >= 0)
        {
            --currentStep;
            DoStep();
        }
    }

    private void DoStep()
    {
        switch (currentStep)
        {
            case 1:
                ToggleState(IntroUIStates.Step1);
                break;
            case 2:
                ToggleState(IntroUIStates.Step2);
                break;
            case 3:
                ToggleState(IntroUIStates.Step3);
                break;
            default:
                ToggleState(IntroUIStates.Start);
                break;
        }
    }

    private void SetUpButtons()
    {
        GameObject.Find("NextButton").GetComponentInChildren<Text>().text = "Next";
        GameObject.Find("PreviousButton").GetComponentInChildren<Text>().text = "Previous";
        GameObject.Find("ConnectionReadyButton").GetComponentInChildren<Text>().text = "Start Calibration";
    }

    private void SetUpSteps()
    {
    }
    
    public void DeviceConnected()
    {
        ToggleState(IntroUIStates.DeviceConnected);
    }

    public void ProceedToCalibration()
    {
        GameManager.Instance.UnLoadScene("Intro");
        GameManager.Instance.LoadScene("Calibration");
    }

    public void ToggleState(IntroUIStates introUIStates)
    {
        switch (introUIStates)
        {
            case IntroUIStates.Start:
                introButtonGroup.SetActive(true);
                introText.text = "This project requires the NextMind brain interface. Get started by connecting your device.";
                break;
            case IntroUIStates.Step1:
                introButtonGroup.SetActive(false);
                break;
            case IntroUIStates.Step2:
                introButtonGroup.SetActive(false);
                break;
            case IntroUIStates.Step3:
                introButtonGroup.SetActive(false);
                break;
            case IntroUIStates.Waiting:
                introButtonGroup.SetActive(false);
                break;
            case IntroUIStates.DeviceConnected:
                introButtonGroup.SetActive(true);
                break;
            default:
                introButtonGroup.SetActive(false);
                Debug.Log("Unknown Intro UI State");
                break;
        }
    }
}


public enum IntroUIStates
{
    Start,
    Step1,
    Step2,
    Step3,
    Waiting,
    DeviceConnected
}