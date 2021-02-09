using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    protected UIManager() {}
    
    public List<Text> noteTexts;

    public void RefreshNoteTexts()
    {
        for (var i = 0; i < NoteManager.Instance.currentScale.Count; ++i)
        {
            noteTexts[i].text = NoteManager.Instance.currentScale[i].note_name;
        }
    }
    
    private void InitializeUI()
    {
        Debug.Log("UI Loaded");
    }
    
    private void Start()
    {
        InitializeUI();
    }
    
    public void HandleKeyRootDropdown(int selection)
    {
        NoteManager.Instance.SetCurrentScaleRoot(69+selection);
    }

    public void HandleKeyModeDropdown(int selection)
    {
        
        var index = 0;
        
        foreach (ScalesAndModes scaleAndMode in Enum.GetValues(typeof(ScalesAndModes)))
        {
            if (index == selection)
            {
               NoteManager.Instance.SetCurrentScaleMode(scaleAndMode);
            }
            ++index;
        }
    }
}
