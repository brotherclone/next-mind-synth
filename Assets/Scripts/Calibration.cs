using System.Collections;
using UnityEngine;
using NextMind;
using NextMind.Calibration;
using NextMind.Devices;

public class Calibration : MonoBehaviour
{
    [SerializeField] private CalibrationManager calibrationManager;

    void Start()
    {
        StartCoroutine(StartCalibrationWhenReady());
        CalibrationUIManager.Instance.ToggleState(CalibrationUIStates.Calibration);
        calibrationManager.SetNeuroTagBehaviour(new CalibrationTagBehavior());
    }

    private IEnumerator StartCalibrationWhenReady()
    {
        yield return new WaitUntil(NeuroManager.Instance.IsReady);
        calibrationManager.StartCalibration();
        calibrationManager.onCalibrationResultsAvailable.AddListener(OnReceivedResults);
    }

    private void OnReceivedResults(Device device, CalibrationResults.CalibrationGrade grade)
    {
        CalibrationUIManager.Instance.SetCalibrationText($"Received results for {device.Name} with a grade of {grade}");

        switch (grade)
        {
            case CalibrationResults.CalibrationGrade.A:
                CalibrationUIManager.Instance.ToggleState(CalibrationUIStates.PostCalibrationSuccess);
                break;
            case CalibrationResults.CalibrationGrade.B:
                CalibrationUIManager.Instance.ToggleState(CalibrationUIStates.PostCalibrationSuccess);
                break;
            case CalibrationResults.CalibrationGrade.C:
                CalibrationUIManager.Instance.ToggleState(CalibrationUIStates.PostCalibrationSuccess);
                break;
            case CalibrationResults.CalibrationGrade.D:
                CalibrationUIManager.Instance.ToggleState(CalibrationUIStates.PostCalibrationFailure);
                break;
            case CalibrationResults.CalibrationGrade.E:
                CalibrationUIManager.Instance.ToggleState(CalibrationUIStates.PostCalibrationFailure);
                break;
        }
    }
}