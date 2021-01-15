using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public string noteDataPath;
    private NoteCollection _noteCollection;

    private void LoadNoteData()
    {
        using (StreamReader stream = new StreamReader(noteDataPath))
        {
            string json = stream.ReadToEnd();
            _noteCollection = JsonUtility.FromJson<NoteCollection>(json);
            for (int i = 0; i < _noteCollection.notes.Length; ++i)
            {
                Debug.Log(_noteCollection.notes[i].frequency);
            }
        }
    }

    private void Start()
    {
        LoadNoteData();
    }
}
