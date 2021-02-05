using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NextMind;
using NextMind.Calibration;
using NextMind.Devices;
using TMPro;


public class Calibration : MonoBehaviour
{
    
    [SerializeField]
    private CalibrationManager calibrationManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCalibrationWhenReady());
        CalibrationUIManager.Instance.ToggleState(CalibrationUIState.Calibration);
        calibrationManager.SetNeuroTagBehaviour(new CalibrationTagBehavior());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator StartCalibrationWhenReady()
    {
        // Waiting for the NeuroManager to be ready
        yield return new WaitUntil(NeuroManager.Instance.IsReady);
       
        // Actually start the calibration process.
        calibrationManager.StartCalibration();
        
        // Listen to the incoming results
        calibrationManager.onCalibrationResultsAvailable.AddListener(OnReceivedResults);
    }
    
    private void OnReceivedResults(Device device, CalibrationResults.CalibrationGrade grade)
    {
        CalibrationUIManager.Instance.SetCalibrationText($"Received results for {device.Name} with a grade of {grade}");
        
        switch (grade)
        {
            case CalibrationResults.CalibrationGrade.A:
                CalibrationUIManager.Instance.ToggleState(CalibrationUIState.PostCalibrationSuccess);
                break;
            case CalibrationResults.CalibrationGrade.B:
                CalibrationUIManager.Instance.ToggleState(CalibrationUIState.PostCalibrationSuccess);
                break;
            case CalibrationResults.CalibrationGrade.C:
                CalibrationUIManager.Instance.ToggleState(CalibrationUIState.PostCalibrationSuccess);
                break;
            case CalibrationResults.CalibrationGrade.D:
                CalibrationUIManager.Instance.ToggleState(CalibrationUIState.PostCalibrationFailure);
                break;
            case CalibrationResults.CalibrationGrade.E:
                CalibrationUIManager.Instance.ToggleState(CalibrationUIState.PostCalibrationFailure);
                break;
        } 
    }
}
