using UnityEngine;
using System;
using JetBrains.Annotations;

[Serializable]
public class Note {

    public int midiNumber;
    public float frequency;
    public int octaveNumber;
    public string noteName;
    [CanBeNull] public string noteAlias;

    public string GETMidiNoteName()
    {
        return noteAlias+octaveNumber;
    }
}