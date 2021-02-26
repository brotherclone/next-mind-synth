using System;

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