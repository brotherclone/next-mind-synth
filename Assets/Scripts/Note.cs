using System;
using JetBrains.Annotations;

[Serializable]
public class Note
{
    public int midi_number;
    public float frequency;
    public int octave_number;
    public string note_name;
    public int keyboard_number;
    [CanBeNull] public string noteAlias;

    public string GETMidiNoteName()
    {
        return note_name + octave_number;
    }
}