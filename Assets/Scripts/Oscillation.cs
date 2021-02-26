using UnityEngine;

public class Oscillation : Singleton<Oscillation>
{
    protected Oscillation()
    {
    }

    private readonly float sampleRate = 44100;
    private float _frequency = 1f;
    private float _amplitude = 0.1f;
    public SignalTypes signalTypes;
    private int _timeIndex = 0;
    private AudioSource _mOscillatorAudioSource;

    private void Start()
    {
        _mOscillatorAudioSource = GetComponent<AudioSource>();
        signalTypes = SignalTypes.Triangle;
        UIManager.Instance.UpdateWaveButtons(SignalTypes.Triangle, true);
    }

    public void SetOscillatorVolume()
    {
        if (NoteManager.Instance != null && _mOscillatorAudioSource != null)
        {
            _mOscillatorAudioSource.volume = NoteManager.Instance.currentVolume;
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (NoteManager.Instance)
        {
            _amplitude = NoteManager.Instance.CurrentVolumeLevel() / 2f;
            _frequency = NoteManager.Instance.CurrentFrequency();
        }
        else
        {
            _amplitude = 0.0f;
            _frequency = 440f;
        }

        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = SignalGeneration(_timeIndex, _frequency, sampleRate, _amplitude);

            if (channels == 2)
                data[i + 1] = data[i];

            _timeIndex++;
        }
    }

    public void SwitchSignalToTriangle()
    {
        signalTypes = SignalTypes.Triangle;
        UIManager.Instance.UpdateWaveButtons(SignalTypes.Triangle, false);
    }

    public void SwitchSignalToSquare()
    {
        signalTypes = SignalTypes.Square;
        UIManager.Instance.UpdateWaveButtons(SignalTypes.Square, false);
    }

    public void SwitchSignalToSawTooth()
    {
        signalTypes = SignalTypes.Sawtooth;
        UIManager.Instance.UpdateWaveButtons(SignalTypes.Sawtooth, false);
    }

    public void SwitchSignalToSine()
    {
        signalTypes = SignalTypes.Sine;
        UIManager.Instance.UpdateWaveButtons(SignalTypes.Sine, false);
    }

    private float SignalGeneration(int timeIndex, float frequency, float sampleRate, float amplitude)
    {
        var signalValue = 0f;
        var t = (frequency * timeIndex) / sampleRate;

        switch (signalTypes)
        {
            case SignalTypes.Sine:
                signalValue = Mathf.Sin(2 * Mathf.PI * t);
                break;
            case SignalTypes.Square:
                signalValue = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * t));
                break;
            case SignalTypes.Triangle:
                signalValue = (1f - 4f * Mathf.Abs(Mathf.Round(t - 0.25f) - (t - 0.25f)));
                break;
            case SignalTypes.Sawtooth:
                signalValue = 2f * (t - Mathf.Floor(t + 0.5f));
                break;
        }

        return (signalValue * amplitude);
    }
}