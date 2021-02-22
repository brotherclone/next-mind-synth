using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NextMind;
using NextMind.NeuroTags;

public class CalibrationTagController : MonoBehaviour
{
    [SerializeField]
    public NeuroTag neuroTag;

    private float _currentConfidenceValue = 0;

    private float _targetConfidenceValue = 0;

    private const float ConfidenceSmoothingSpeed = 5;

    
    private void Awake()
    {
        if (neuroTag == null)
        {
            neuroTag = GetComponentInParent<NeuroTag>();
            SetUpListeners();
        }else
        {
            SetUpListeners();
        }
    }
    
    private void SetUpListeners()
    {
        neuroTag.onConfidenceChanged.AddListener(OnConfidenceUpdated);
        neuroTag.onTriggered.AddListener(OnTagTrigger);
    }
    
    private void OnTagTrigger()
    {
        CalibrationUIManager.Instance.TagTriggered();
    }
    
    private void Update()
    {
        HandleConfidenceUpdate();
    }
    
    private void HandleConfidenceUpdate()
    {
        _currentConfidenceValue = Mathf.Lerp(_currentConfidenceValue, _targetConfidenceValue, ConfidenceSmoothingSpeed * Time.deltaTime); 
        CalibrationUIManager.Instance.UpdateConfidence(_currentConfidenceValue);
    }
    
    private void OnConfidenceUpdated(float value)
    {
        _targetConfidenceValue = value;
    }

}
