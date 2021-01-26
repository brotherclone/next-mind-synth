using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NextMind;
using NextMind.NeuroTags;

public class DroneTagController : MonoBehaviour
{
    [SerializeField]
    public NeuroTag neuroTag;

    [SerializeField] 
    public string tagName;

    private float _currentConfidenceValue = 0;

    private float _targetConfidenceValue = 0;

    private const bool _interpolateConfidenceValue = true;

    private const float confidenceSmoothingSpeed = 5;

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
        neuroTag.onBecameActivated.AddListener(OnActivated);
        neuroTag.onBecameDeactivated.AddListener(OnDeactivated);
    }

    private void OnActivated()
    {
        Debug.Log(tagName+ " activated");
    }

    private void OnDeactivated()
    {
        Debug.Log(tagName+ " deactivated");
    }
    
    private void OnTagTrigger()
    {
        Debug.Log(tagName+ " triggered");
        DroneManager.Instance.isToggled = true;
        DroneManager.Instance.isPlaying = true;
        UIManager.Instance.UpDateInfoTexts(InfoText.CurrentDrones, "On");
    }


    private void Update()
    {
  
        HandleConfidenceUpdate();
        if (!neuroTag.IsActive)
        {
            Debug.Log("something is wrong with "+tagName);
        }
        
    }
    private void HandleConfidenceUpdate()
    {
        if (_interpolateConfidenceValue)
        {
            _currentConfidenceValue = Mathf.Lerp(_currentConfidenceValue, _targetConfidenceValue, confidenceSmoothingSpeed * Time.deltaTime);
        }
        else
        {
            _currentConfidenceValue = _targetConfidenceValue;
        }
        
    }
    
    private void OnConfidenceUpdated(float value)
    {
        //Debug.Log("Tag confidence "+ value);
        _targetConfidenceValue = value;
    }
    
}
