using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject leftPanel;
    public TMP_Text statusText;
    private List<GameObject> _panels = new List<GameObject>();
    private Animator _currentAnimator;
 
    private void InitializeUI()
    {
        _panels.Add(leftPanel);
        statusText.text = "Loaded";
        Debug.Log("UI Loaded");
    }

    public void TogglePanel(PanelName panelName, TagUIInteraction _tagUIInteraction)
    {
        switch (panelName)
        {
            case PanelName.LeftPanel:
                _currentAnimator = leftPanel.GetComponent<Animator>();
                var status = _currentAnimator.GetBool("isDisplayed");
                _currentAnimator.SetBool("isDisplayed", !status);
                _tagUIInteraction.EnableDisableTag(true);
                break;
        }
    }
    
    private void Start()
    {
        InitializeUI();
    }

    private void Update()
    {
        
    }

    private bool AnimationFinished()
    {
        return _currentAnimator.GetCurrentAnimatorStateInfo(0).length >
               _currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}

public enum UIActions{
    Open,
    Close
}

public enum UIType
{
    Panel
}

public enum PanelName
{
    None,
    LeftPanel,
    RightPanel,
    BottomPanel,
    MainPanel
}
