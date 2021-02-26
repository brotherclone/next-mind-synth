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
    private ScalesAndModes _currentScaleMode;
    private Note _currentScaleNote;
    private int _currentRootMidiNumber;
    private string _currentScaleName;
    public OSC osc;
    public bool isTransmittingOSC;
    public Text oscMonitorText;
    private OSCInfo _oscInfo;
    [SerializeField]
    private string oscAddress;
    
    private void Start()
    {
        LoadNoteData();
        SetVolume(0.25f);
    }

    public void SetUpOSCInfo(int _in, int _out, string _port)
    {
        _oscInfo = new OSCInfo(_in, _out, _port, oscAddress);
    }
    
    private void LoadNoteData()
    {
        using (StreamReader stream = new StreamReader(_noteDataPath))
        {
            string json = stream.ReadToEnd();
            _noteCollection = JsonUtility.FromJson<NoteCollection>(json);
            for (int i = 0; i < _noteCollection.notes.Length; ++i)
            {
               notes.Add( _noteCollection.notes[i]);
            }
        }
        MakeScale(69, ScalesAndModes.Major);
    }


    public void TurnOSCOnOff(bool isOn)
    {
        isTransmittingOSC = isOn;
        UIManager.Instance.UpdateOSCButtons(isOn); 
        oscMonitorText.gameObject.SetActive(isOn);
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
        SetCurrentNote(0);
        DroneManager.Instance.SetDroneNote(currentScale[0]);
        UIManager.Instance.RefreshNoteTexts();
    }

    private void GetMidiNoteFromNumber(int noteNumber)
    {
        foreach (var n in notes.Where(n => n.midi_number == noteNumber))
        {
            _currentScaleNote = n;
        }
    }

    public void SetCurrentNote(int position)
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
    }


    private void TransmitOSC(Note note)
    {
        var _oscMessage = new OscMessage();
        _oscMessage.address = oscAddress;
        _oscMessage.values.Add(note.midi_number);
        var vol = CurrentVolumeToMIDI();
        _oscMessage.values.Add(vol);
        osc.Send(_oscMessage);
        oscMonitorText.text = _oscInfo.MakeMessage(note.midi_number, vol);
    }

    private int CurrentVolumeToMIDI()
    {
        var volumePercent = Math.Floor(currentVolume * 100);
        var m = Math.Round(volumePercent * 127 * 0.01);
        return (int) m;
    }
    
    public float CurrentFrequency()
    {
        return currentNote.frequency;
    }

    public float CurrentVolumeLevel()
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
        currentVolume = volume;
        var volumePercent = Math.Floor(volume * 100);
        DroneManager.Instance.SetDroneVolume();
        Oscillation.Instance.SetOscillatorVolume();
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

public class OSCInfo
{
    private int inPort, outPort, midiNumber, volume;
    private string outIP, outAddress;


    public OSCInfo(int _inPort, int _outPort, string _outIP, string _outAddress)
    {
        inPort = _inPort;
        outPort = _outPort;
        outIP = _outIP;
        outAddress = _outAddress;
        midiNumber = 0;
        volume = 0;
    }
    
    public String MakeMessage(int _midi, int _volume)
    {
        midiNumber = _midi;
        volume = _volume;
        return outIP+"/"+outPort+"/"+outAddress+" MIDI:"+midiNumber+", "+volume;
    }
}