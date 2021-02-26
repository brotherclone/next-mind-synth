using System;
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
    
    private float confidenceSmoothingSpeed = 5;

    public GameObject backgroundImage;
    
    private float _alpha = 0;

    private void Start()
    {
        backgroundImage.SetActive(false);
    }

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
        NoteManager.Instance.SetCurrentNote(position);
    }
    
    private void Update()
    {
        HandleConfidenceUpdate();
    }
    
    private void HandleConfidenceUpdate()
    {
        _currentConfidenceValue = Mathf.Lerp(_currentConfidenceValue, _targetConfidenceValue, confidenceSmoothingSpeed * Time.deltaTime);
        // ToDo: This seems like an issue in Unity. Even with it's own material, alpha effects all UI elements.
        // _alpha = 1f * _currentConfidenceValue;
        // var img = backgroundImage.GetComponent<Image>();
        // var col = img.material.color;
        // col.a = _alpha;
        // img.material.color = col;

        if (_currentConfidenceValue > 0.05f)
        {
            backgroundImage.SetActive(true);
        }
        else
        {
            backgroundImage.SetActive(false);
        }
    }
    
    private void OnConfidenceUpdated(float value)
    {
        //Debug.Log("Tag confidence "+ value);
        _targetConfidenceValue = value;
    }
    
}
