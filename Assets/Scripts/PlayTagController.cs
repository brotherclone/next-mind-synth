using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NextMind;
using NextMind.NeuroTags;

public class PlayTagController : MonoBehaviour
{
    [SerializeField]
    public NeuroTag neuroTag;

    [SerializeField] 
    public string playTagName;
    
    public int position;
    
    private float _currentConfidenceValue = 0;

    private float _targetConfidenceValue = 0;

    private bool _interpolateConfidenceValue = true;

    private float confidenceSmoothingSpeed = 5;
    
    private void Awake()
    {
        if (neuroTag == null)
        {
            neuroTag = GetComponentInChildren<NeuroTag>();
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
        //Debug.Log(playTagName+ " activated");
    }

    private void OnDeactivated()
    {
        //Debug.Log(playTagName+ " deactivated");
    }
    
    private void OnTagTrigger()
    {
        Debug.Log("trigger");
        NoteManager.Instance.setCurrentNote(position);
    }
    
    private void Update()
    {
        HandleConfidenceUpdate();
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

        if (_currentConfidenceValue > 0.075f)
        {
            FocusParticleManager.Instance.UpdateFocusParticles(neuroTag.transform.position);
        }
    }
    
    private void OnConfidenceUpdated(float value)
    {
        //Debug.Log("Tag confidence "+ value);
        _targetConfidenceValue = value;
    }
    
}
