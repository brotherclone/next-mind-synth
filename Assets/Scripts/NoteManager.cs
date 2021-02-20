using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : Singleton<NoteManager>
{
    protected NoteManager() {}
    private readonly string _noteDataPath = Application.streamingAssetsPath + "/notes.json";
    private NoteCollection _noteCollection;
    public List<Note> notes;
    public List<Note> currentScale;

    [SerializeField] private Text readyText;

    public string currentScaleName;
    /*
    Major	    T T S T T T S
    Minor	    T S T T S T T
    Ionian	    T T S T T T S
    Dorian	    T S T T T S T
    Phrygian	S T T T S T T
    Lydian	    T T T S T T S
    Mixolydian	T T S T T S T
    Aeolian	    T S T T S T T
    Locrian	    S T T S T T T
    */
    
    private readonly int[] _majorSteps = new[] {2, 2, 1, 2, 2, 2, 1};
    private readonly int[] _minorSteps = new[] {2, 1, 2, 2, 1, 2, 2};
    private readonly int[] _ionianSteps = new[] {2, 2, 1, 2, 2, 2, 1};
    private readonly int[] _dorianSteps = new[] {2, 1, 2, 2, 2, 1, 2};
    private readonly int[] _phrygianSteps = new[] {1, 2, 2, 2, 1, 2, 2};
    private readonly int[] _lydianSteps = new[] {2, 2, 2, 1, 2, 2, 1};
    private readonly int[] _mixolydianSteps = new[] {2, 2, 1, 2, 2, 1, 2};
    private readonly int[] _aeolianSteps = new[] {2, 1, 2, 2, 1, 2, 2};
    private readonly int[] _locrianSteps = new[] {1, 2, 2, 1, 2, 2, 2};
    private int[] _currentInterval = new int[8];

    public Note currentNote;
    public float currentVolume;
    private float _previousVolume;
    private bool _isMuted;
    private ScalesAndModes _currentScaleMode;
    private Note _currentScaleNote;
    private int _currentRootMidiNumber;
    private string _currentScaleName;
    public OSC osc;
    public bool isTransmittingOSC;
    
    private void Start()
    {
        LoadNoteData();
        SetVolume(0.025f);
    }
    
    
    private void LoadNoteData()
    {
        using (StreamReader stream = new StreamReader(_noteDataPath))
        {
            string json = stream.ReadToEnd();
            readyText.text = json;
            _noteCollection = JsonUtility.FromJson<NoteCollection>(json);
            for (int i = 0; i < _noteCollection.notes.Length; ++i)
            {
               notes.Add( _noteCollection.notes[i]);
               readyText.text = _noteCollection.notes[i].note_name;
            }
        }
        MakeScale(69, ScalesAndModes.Major);
    }
    
    public void TurnOnOSC()
    {
        isTransmittingOSC = true;
        UIManager.Instance.UpdateOSCButtons(true);
    }

    public void TurnOffOSC()
    {
        isTransmittingOSC = false;
        UIManager.Instance.UpdateOSCButtons(false);
    }

    private void GetMidiNotesFromNumbers(IReadOnlyList<int> noteNumbers)
    {
        foreach (var t in noteNumbers)
        {
            foreach (var n in notes.Where(n => n.midi_number == t))
            {
                currentScale.Add(n);
            }
        }

        setCurrentNote(0);
        UIManager.Instance.RefreshNoteTexts();
    }

    private void GetMidiNoteFromNumber(int noteNumber)
    {
        foreach (var n in notes.Where(n => n.midi_number == noteNumber))
        {
            _currentScaleNote = n;
        }
    }

    public void setCurrentNote(int position)
    {
        if (position <= currentScale.Count)
        {
            currentNote = currentScale[position];

            if (isTransmittingOSC)
            {
                TransmitOSC(currentNote);
            }
        }
        else
        {
            currentNote = currentScale[0];
        }

        readyText.text = currentNote.note_name;
    }


    private void TransmitOSC(Note note)
    {
        var _oscMessage = new OscMessage();
        _oscMessage.address = "/NextMindSynth/Cantor";
        _oscMessage.values.Add(note.midi_number);
        var vol = CurrentVolumeToMIDI();
        _oscMessage.values.Add(vol);
        osc.Send(_oscMessage);
    }

    private int CurrentVolumeToMIDI()
    {
        var volumePercent = Math.Floor(currentVolume * 100);
        var m = Math.Round(volumePercent * 127 * 0.01);
        return (int) m;
    }
    
    public float currentFrequency()
    {
        return currentNote.frequency;
    }

    public float currentVolumeLevel()
    {
        return currentVolume;
    }

    public void SetCurrentScaleRoot(int midiValue)
    {
        MakeScale(midiValue, _currentScaleMode);
    }

    public void SetCurrentScaleMode(ScalesAndModes scaleMode)
    {
        MakeScale(_currentRootMidiNumber, scaleMode);
    }
    
    private void MakeScale(int startMidiNumber, ScalesAndModes scalesAndModes)
    {
        readyText.text = "MakeScale";
        currentScale.Clear();
        _currentScaleMode = scalesAndModes;
        _currentRootMidiNumber = startMidiNumber;

        GetMidiNoteFromNumber(startMidiNumber);
        
        switch (scalesAndModes)
        {
            case ScalesAndModes.Aeolian:
                _currentInterval = _aeolianSteps;
                break;
            case ScalesAndModes.Dorian:
                _currentInterval = _dorianSteps;
                break;
            case ScalesAndModes.Ionian:
                _currentInterval = _ionianSteps;
                break;
            case ScalesAndModes.Locrian:
                _currentInterval = _locrianSteps;
                break;
            case ScalesAndModes.Lydian:
                _currentInterval = _lydianSteps;
                break;
            case ScalesAndModes.Minor:
                _currentInterval = _minorSteps;
                break;
            case ScalesAndModes.Mixolydian:
                _currentInterval = _mixolydianSteps;
                break;
            case ScalesAndModes.Phrygian:
                _currentInterval = _phrygianSteps;
                break;
            default:
                _currentInterval = _majorSteps;
                break;
        }
        
        var stepCounter = 0;
        var midiNoteCounter = startMidiNumber;
        var scaleMidiNumbers = new List<int>();
        scaleMidiNumbers.Add(midiNoteCounter);
        while (stepCounter < _currentInterval.Length)
        {
            if (midiNoteCounter + _currentInterval[stepCounter] >= 127) continue;
            midiNoteCounter += _currentInterval[stepCounter];
            scaleMidiNumbers.Add(midiNoteCounter);
            ++stepCounter;
            // foreach (var num in scaleMidiNumbers)
            // {
            //     Debug.Log("checking number sequence -> " + num);
            // }
        }
        GetMidiNotesFromNumbers(scaleMidiNumbers);
    }
    
    public void SetVolume(float volume)
    {
        if (volume <= 0.0 && _isMuted != true)
        {
            Mute();
        }
        else
        {
            currentVolume = volume;
            var volumePercent = Math.Floor(volume * 100);
        }
    }

    public void Mute()
    {
        if (_isMuted != true)
        {
            _previousVolume = currentVolume;
            SetVolume(0f);
            _isMuted = true;
        }
        else
        {
            SetVolume(_previousVolume);
            _isMuted = false;
        }
       
    }

}

public enum ScalesAndModes
{
    Major,
    Minor,
    Ionian,
    Dorian,
    Phrygian,
    Lydian,
    Mixolydian,
    Aeolian,
    Locrian
}