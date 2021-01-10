using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NextMind;
using NextMind.NeuroTags;

public class TagManager : MonoBehaviour
{
    public UIManager _UIManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TagDoComment()
    {
        Debug.Log("Tag Action");
        _UIManager.OpenPanel();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
