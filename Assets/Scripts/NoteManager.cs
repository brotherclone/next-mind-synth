using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class NoteManager : Singleton<NoteManager>
{
    protected NoteManager() {}
    public string noteDataPath;
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
    private bool _isMuted;
    private ScalesAndModes _currentScaleMode;
    private Note _currentScaleNote;
    private string _currentScaleName;
    public OSC osc;
    
    private void LoadNoteData()
    {
        using (StreamReader stream = new StreamReader(noteDataPath))
        {
            string json = stream.ReadToEnd();
            _noteCollection = JsonUtility.FromJson<NoteCollection>(json);
            for (int i = 0; i < _noteCollection.notes.Length; ++i)
            {
               notes.Add( _noteCollection.notes[i]);
            }
        }
        MakeScale(69, ScalesAndModes.Minor);
    }

    private void Start()
    {
        LoadNoteData();

    }

    private void GetMidiNotesFromNumbers(IReadOnlyList<int> noteNumbers)
    {
        foreach (var t in noteNumbers)
        {
            foreach (var n in notes.Where(n => n.midi_number == t))
            {
                currentScale.Add(n);
                Debug.Log(n.note_name + " ADDED");
            }
        }
        setCurrentNote(0); 
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
            UIManager.Instance.UpDateInfoTexts(InfoText.Triggering, currentNote.note_name);
            TransmitOSC(currentNote);
        }
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
    
    private void SetCurrentScale()
    {
        _currentScaleName = _currentScaleNote.note_name + " " + _currentScaleMode.ToString();
        UIManager.Instance.UpDateInfoTexts(InfoText.CurrentKey, _currentScaleName);
    }

    public float currentFrequency()
    {
        return currentNote.frequency;
    }

    public float currentVolumeLevel()
    {
        return currentVolume;
    }
    
    private void MakeScale(int startMidiNumber, ScalesAndModes scalesAndModes)
    {
        _currentScaleMode = scalesAndModes;
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
            foreach (var num in scaleMidiNumbers)
            {
                Debug.Log("checking number sequence -> " + num);
            }
        }
        GetMidiNotesFromNumbers(scaleMidiNumbers);
        SetCurrentScale();
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
            UIManager.Instance.UpDateInfoTexts(InfoText.Volume, volumePercent.ToString(CultureInfo.CurrentCulture)+"%");
        }
    }

    public void Mute()
    {
        if (_isMuted != true)
        {
            _previousVolume = currentVolume;
            SetVolume(0f);
            _isMuted = true;
            UIManager.Instance.UpDateInfoTexts(InfoText.Volume, "!MUTE!");
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