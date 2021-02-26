using UnityEngine;
using NextMind.NeuroTags;

public class PlayTagController : MonoBehaviour
{
    [SerializeField] public NeuroTag neuroTag;
    [SerializeField] public string playTagName;
    public int position;
    private float _currentConfidenceValue = 0;
    private float _targetConfidenceValue = 0;
    private const float ConfidenceSmoothingSpeed = 5;

    public GameObject backgroundImage;
    //private float _alpha = 0;

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
        }
        else
        {
            SetUpListeners();
        }
    }


    private void SetUpListeners()
    {
        neuroTag.onConfidenceChanged.AddListener(OnConfidenceUpdated);
        neuroTag.onTriggered.AddListener(OnTagTrigger);
    }

    private void OnTagTrigger()
    {
        NoteManager.Instance.SetCurrentNote(position);
    }

    private void Update()
    {
        HandleConfidenceUpdate();
    }

    private void HandleConfidenceUpdate()
    {
        _currentConfidenceValue = Mathf.Lerp(_currentConfidenceValue, _targetConfidenceValue,
            ConfidenceSmoothingSpeed * Time.deltaTime);
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
        _targetConfidenceValue = value;
    }
}