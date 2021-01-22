using UnityEngine;
using NextMind.NeuroTags;

public class TagUIInteraction :  MonoBehaviour
{
    [SerializeField]
    private NeuroTag neuroTag = null;

    private float _currentConfidenceValue = 0;

    private float _targetConfidenceValue = 0;

    private bool _interpolateConfidenceValue = true;

    [SerializeField]
    private float confidenceSmoothingSpeed = 5;

    public UIManager _UIManager;

    public UIType _UIType;

    public PanelName _PanelName;

    private bool _thinkable = true;
    
    private void Update()
    {
  
        HandleConfidenceUpdate();
        
    }

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
        InteractWithUI();
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

    public void EnableDisableTag(bool shouldEnable)
    {
        if(shouldEnable == true)
        {
            _thinkable = true;
        }
        else
        {
            _thinkable = false; 
            
        }

        _currentConfidenceValue = 0;
        _targetConfidenceValue = 0;
    }

    private void InteractWithUI()
    {
        if (_thinkable == false)
        {
            return;
        }
        switch (_UIType)
        {
            case UIType.Panel:
                if (_PanelName != PanelName.None && _thinkable == true)
                {
                    TogglePanel();
                }
                else
                {
                    Debug.LogError("[TagUIInteraction] Tag attempting panel interaction without a target panel.");
                }
                break;
        }
    }

    private void OnDestroy()
    {
        if (neuroTag != null)
        {
            neuroTag.onConfidenceChanged.RemoveListener(OnConfidenceUpdated);
            neuroTag.onTriggered.RemoveListener(OnTagTrigger);
        }
    }
    
    
    private void TogglePanel()
    {
        EnableDisableTag(false);
        _UIManager.TogglePanel(_PanelName, this);
    }
    
    private void OnConfidenceUpdated(float value)
    {
        if (_thinkable == true)
        { 
            _targetConfidenceValue = value; 
        }
    }
}
