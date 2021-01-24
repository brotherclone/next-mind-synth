using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilation : MonoBehaviour
{
    private readonly float sampleRate = 44100;
    private float _frequency = 1f;
    private float _amplitude = 0.1f;
    public SignalType signalType = SignalType.Sine;
    private int _timeIndex = 0;

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (NoteManager.Instance)
        {
            _amplitude = NoteManager.Instance.currentVolumeLevel();
            _frequency = NoteManager.Instance.currentFrequency();
        }
        
        for (int i = 0; i < data.Length; i += channels)
        {
            
            data[i] = SignalGeneration(_timeIndex, _frequency, sampleRate, _amplitude);
            
            if (channels == 2)
                data[i + 1] = data[i];

            _timeIndex++;
        }
    }


    public float SignalGeneration(int timeIndex, float frequency, float sampleRate, float amplitude)
    {
        float signalValue = 0f;
        float t = (frequency * timeIndex) / sampleRate;
        
        switch (signalType)
        {
            case SignalType.Sine:
                //sin( 2 * pi * t )
                signalValue = Mathf.Sin(2 * Mathf.PI * t);
                break;
            case SignalType.Square:
                //sign( sin( 2 * pi * t ) )
                signalValue = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * t ));
                break;
            case SignalType.Triangle:
                // 2 * abs( t - 2 * floor( t / 2 ) - 1 ) - 1
                signalValue = (1f-4f * Mathf.Abs( Mathf.Round(t-0.25f)-(t-0.25f)));
                break;
            case SignalType.Sawtooth:
                // 2 * ( t/a - floor( t/a + 1/2 ) )
                signalValue = 2f * (t - Mathf.Floor(t + 0.5f));
                break;
        }
        return (signalValue * amplitude);
    }
}

public enum SignalType
{
    Sine,
    Square,
    Triangle,
    Sawtooth
}