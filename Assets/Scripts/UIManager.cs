using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.EditorUtilities;

public class UIManager : Singleton<UIManager>
{
    protected UIManager() {}
    
   
    
    private void InitializeUI()
    {
        Debug.Log("UI Loaded");
    }
    
    
    
    private void Start()
    {
        InitializeUI();
    }
    
}
