using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonePlayer : DeviceComponent
{
    public TonePlayer(TwinObject device){
        this.device = device;
    }

    public override void update(){
    }

    public void playTone(int frequency){
        byte[] freqbytes = BitConverter.GetBytes(frequency);
        device.sendActionMessage("toneplayer/play", freqbytes);
    }

    public void playTone(int frequency, int duration){
        byte[] freqbytes = BitConverter.GetBytes(frequency);
        byte[] durbytes = BitConverter.GetBytes(duration);
        List<byte> bytesArray = new List<byte>();
        bytesArray.AddRange(freqbytes);
        bytesArray.AddRange(durbytes);
        device.sendActionMessage("toneplayer/frequency_duration", bytesArray.ToArray());
    }

    public void stopTone(){
        device.sendActionMessage("toneplayer/stop", 0);
    }
}
