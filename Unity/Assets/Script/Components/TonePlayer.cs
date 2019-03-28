using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonePlayer : DeviceComponent
{
    public void PlayTone(int frequency){
        List<byte> bytesArray = new List<byte>();
        byte[] freqbytes = BitConverter.GetBytes(frequency);
        bytesArray.Add(0);
        bytesArray.AddRange(freqbytes);
        device.SendActionMessage("toneplayer/play", freqbytes);
    }

    public void PlayTone(int frequency, int duration){
        byte[] freqbytes = BitConverter.GetBytes(frequency);
        byte[] durbytes = BitConverter.GetBytes(duration);
        List<byte> bytesArray = new List<byte>();
        bytesArray.AddRange(freqbytes);
        bytesArray.AddRange(durbytes);
        device.SendActionMessage("toneplayer/frequency_duration", bytesArray.ToArray());
    }

    public void StopTone(){
        device.SendActionMessage("toneplayer/stop", 0);
    }
}
