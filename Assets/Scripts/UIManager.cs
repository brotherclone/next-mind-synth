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

    public void OpenPanel()
    { 
        _currentAnimator = leftPanel.GetComponent<Animator>();
        _currentAnimator.SetBool("isDisplayed", true);
    }
    
    private void Start()
    {
        InitializeUI();
    }
}



