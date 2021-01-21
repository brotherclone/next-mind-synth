using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class NoteManager : MonoBehaviour
{
    public string noteDataPath;
    private NoteCollection _noteCollection;
    public List<Note> notes;
    public List<Note> currentScale;
    
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
        MakeMajor(60);
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
                Debug.Log(n.note_name);
            }
        }
    }

    private void MakeMajor(int startMidiNumber)
    {
        //T-T-S-T-T-T-S,
        var majorSteps = new[] { 0,2,2,1,2,2,2,1 };
        var majorStepCounter = 0;
        var midiNoteCounter = startMidiNumber;
        var scaleMidiNumbers = new List<int>();
        while (majorStepCounter < majorSteps.Length)
        {
            scaleMidiNumbers.Add(midiNoteCounter);
            if (midiNoteCounter + majorSteps[majorStepCounter] >= 127) continue;
            midiNoteCounter += majorSteps[majorStepCounter];
            ++majorStepCounter;
        }
        GetMidiNotesFromNumbers(scaleMidiNumbers);
    }
}
