using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NextMind;
using NextMind.NeuroTags;

public class PlayTagController : MonoBehaviour
{
    public NeuroTag neuroTag;
    
    public int position;
    
    private float _currentConfidenceValue = 0;

    private float _targetConfidenceValue = 0;

    //private bool _interpolateConfidenceValue = true;

    
    
    private void Awake()
    {
        if (neuroTag == null)
        {
            neuroTag = GetComponentInParent<NeuroTag>();
        }
        if (neuroTag != null)
        {
            neuroTag.onConfidenceChanged.AddListener(OnConfidenceUpdated);
            neuroTag.onTriggered.AddListener(OnTagTrigger);
        }
    }
    
    private void OnTagTrigger()
    {
        Debug.Log("Triggered");
        NoteManager.Instance.setCurrentNote(position);
    }
    
    private void OnConfidenceUpdated(float value)
    {
        //Debug.Log("Tag confidence "+ value);
        _targetConfidenceValue = value; 
        
    }
    
}
