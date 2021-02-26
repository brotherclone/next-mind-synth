using UnityEngine;
using UnityEngine.UI;

public class IntroManager : Singleton<IntroManager>
{
    protected IntroManager()
    {
    }

    [SerializeField] private Button startButton;

    [SerializeField] private Text startText;

    public Text versionText;

    private void Start()
    {
        SetUpButtons();
        ToggleState(IntroUIStates.Start);
        versionText.text = "Version: " + Application.version;
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
            case IntroUIStates.Waiting:
                startText.text = "Initializing.";
                startButton.gameObject.SetActive(false);
                break;
            case IntroUIStates.Start:
                startText.text = "Searching for NextMind device. This application requires a NextMind device.";
                startButton.gameObject.SetActive(false);
                break;
            case IntroUIStates.DeviceConnected:
                startText.text = "NextMind available. Proceed to calibration";
                startButton.gameObject.SetActive(true);
                break;
            default:
                Debug.Log("Unknown Intro UI State.");
                break;
        }
    }
}