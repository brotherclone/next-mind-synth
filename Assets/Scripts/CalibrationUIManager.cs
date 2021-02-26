using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NextMind.Calibration;

public class CalibrationUIManager : Singleton<CalibrationUIManager>
{
    protected CalibrationUIManager()
    {
    }

    public GameObject calibrationButtonGroup;

    public GameObject postCalibrationButtonGroup;

    public Text calibrationText;

    public string calibrationTextMessage;

    public int currentCalibrationTagIndex;

    [SerializeField] private List<GameObject> calibrationTagCounters;

    [SerializeField] private List<float> calibrationConfidence;

    public CalibrationManager calibrationManager;

    private GameObject _currentCounter;

    private bool _countersAnimating = false;

    private Vector3 _previousCounterScale;

    private void Start()
    {
        ToggleState(CalibrationUIStates.PreCalibration);
        calibrationManager.SetNeuroTagBehaviour(new CalibrationTagBehavior());
    }

    public void AdvanceTagCounter()
    {
        var currentSpriteObj = calibrationTagCounters[currentCalibrationTagIndex];
        ScaleBounce(currentSpriteObj);
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

    private void ScaleBounce(GameObject bouncer)
    {
        _countersAnimating = true;
        _previousCounterScale = bouncer.transform.localScale;
        _currentCounter = bouncer;
        if (currentCalibrationTagIndex > 0)
        {
            var previousSpriteObj = calibrationTagCounters[currentCalibrationTagIndex - 1];
            previousSpriteObj.transform.localScale = _previousCounterScale;
        }
    }

    private void Update()
    {
        if (_countersAnimating)
        {
            if (_currentCounter.transform.transform.localScale.x > 8.0f)
            {
                _currentCounter.transform.localScale = Vector3.Lerp(_currentCounter.transform.localScale,
                    _currentCounter.transform.localScale * 1.05f, Time.deltaTime * 5);
            }
        }
    }

    public void UpdateConfidence(float currentConfidence)
    {
        calibrationConfidence[currentCalibrationTagIndex] = currentConfidence;
    }

    public void TagCounterAnimate()
    {
        var currentSpriteObj = calibrationTagCounters[currentCalibrationTagIndex];
        var currentSprite = currentSpriteObj.GetComponent<SpriteRenderer>();
        currentSprite.color = new Color(1, 1, 1, calibrationConfidence[currentCalibrationTagIndex]);
    }

    private void SetUpCounters()
    {
        currentCalibrationTagIndex = 0;
        foreach (var t in calibrationTagCounters)
        {
            var currentSprite = t.GetComponent<SpriteRenderer>();
            currentSprite.color = new Color(1, 1, 1, 0.25f);
            calibrationConfidence.Add(0.0f);
        }
    }

    public void DeviceFound()
    {
        ToggleState(CalibrationUIStates.Calibration);
        calibrationManager.StartCalibration();
    }

    private void SetUpButtons()
    {
        GameObject.Find("StartOverButton").GetComponentInChildren<Text>().text = "Start Over";
        GameObject.Find("SkipButton").GetComponentInChildren<Text>().text = "Skip Calibration";
        GameObject.Find("TryAgainButton").GetComponentInChildren<Text>().text = "Try Again";
        GameObject.Find("GetHelpButton").GetComponentInChildren<Text>().text = "Get Help";
        GameObject.Find("ProceedButton").GetComponentInChildren<Text>().text = "Proceed Anyway";
    }

    public void ToggleState(CalibrationUIStates calibrationUIStates)
    {
        switch (calibrationUIStates)
        {
            case CalibrationUIStates.PreCalibration:
                SetUpButtons();
                SetUpCounters();
                calibrationButtonGroup.SetActive(true);
                postCalibrationButtonGroup.SetActive(false);
                calibrationText.text = "Looking for NextMind";
                break;
            case CalibrationUIStates.Calibration:
                calibrationButtonGroup.SetActive(true);
                postCalibrationButtonGroup.SetActive(false);
                calibrationText.text = "Concentrate on each tag to calibrate";
                break;
            case CalibrationUIStates.PostCalibrationFailure:
                calibrationButtonGroup.SetActive(false);
                postCalibrationButtonGroup.SetActive(true);
                calibrationText.text = calibrationTextMessage;
                break;
            case CalibrationUIStates.PostCalibrationSuccess:
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
        ToggleState(CalibrationUIStates.Calibration);
    }

    public void SkipButtonAction()
    {
        ToggleState(CalibrationUIStates.PostCalibrationSuccess);
    }

    public void GetHelpButtonAction()
    {
        Application.OpenURL("https://www.next-mind.com/faq/");
    }

    public void ProceedButtonAction()
    {
        ToggleState(CalibrationUIStates.PostCalibrationSuccess);
    }
}

