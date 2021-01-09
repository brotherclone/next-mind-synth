using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text statusText;
    
    public List<GameObject> panels;
    
    // Start is called before the first frame update
    private Animator currentAnimator;
    void Start()
    {
        statusText.text = "Loaded.";
        
        // Initialize UI Panels
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false); 
            currentAnimator = panel.GetComponent<Animator>();
            currentAnimator.SetBool("isDisplayed", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
