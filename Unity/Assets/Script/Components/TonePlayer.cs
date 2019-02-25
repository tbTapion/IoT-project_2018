using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonePlayer : DeviceComponent
{
    private int duration, frequency;
    public TonePlayer(TwinObject device){
        this.device = device;
    }

    public override void update(){
    }

    public void setDuration(int duration){
        this.duration = duration;
    }

    public void setFrequency(int frequency){
        this.frequency = frequency;
    }

    public int getDuration(){
        return duration;
    }

    public int getFrequency(){
        return frequency;
    }

    public void playTone(){
        device.sendActionMessage("toneplayer/", )
    }
}
