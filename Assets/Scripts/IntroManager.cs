using UnityEngine;
using UnityEngine.UI;

public class IntroManager : Singleton<IntroManager>
{
    protected IntroManager() {}

    [SerializeField] private GameObject startButton;

    [SerializeField] private Text startText;

    public Text versionText;
    
    private void Start()
    {
        SetUpButtons();
        ToggleState(IntroUIStates.Start);
        versionText.text = "Version: " +Application.version;
    }

    private void Awake()
    {
        ToggleState(IntroUIStates.Waiting);
    }
    
    private void SetUpButtons()
    {
        startButton.GetComponentInChildren<Text>().text = "Start Calibration";
    }
    
    public void DeviceConnected()
    {
        ToggleState(IntroUIStates.DeviceConnected);
    }

    public void ProceedToCalibration()
    {
        GameManager.Instance.UnLoadScene("Intro");
        GameManager.Instance.LoadScene("Calibration");
    }

    private void ToggleState(IntroUIStates introUIStates)
    {
        switch (introUIStates)
        {
            case IntroUIStates.Start:
                startText.text = "Initializing";
                startButton.SetActive(false);
                break;
            case IntroUIStates.Waiting:
                startText.text = "Searching for NextMind device";
                startButton.SetActive(false);
                break;
            case IntroUIStates.DeviceConnected:
                startText.text = "NextMind available. Proceed to calibration";
                startButton.SetActive(true);
                break;
            default:
                Debug.Log("Unknown Intro UI State");
                break;
        }
    }
}


public enum IntroUIStates
{
    Start,
    Waiting,
    DeviceConnected
}