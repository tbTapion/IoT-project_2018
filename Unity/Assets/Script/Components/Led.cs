using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Led : DeviceComponent {

    protected bool state;
    protected bool heartbeat;
    protected int heartbeatTime;

    public Led(TwinObject device){
        this.device = device;
        state = false;
    }

    public override void update(){

    }

    public void toggle()
    {
        setState(!state);
    }

    public void setState(bool state)
    {
        this.state = state;
        device.sendActionMessage("led", (state ? 1 : 0).ToString());
    }

    public bool getState(){
        return state;
    }

    public void setHeartbeatTime(int heartbeatTime)
    {
        int tempHeartbeatTime = Mathf.Min(heartbeatTime, 70);
        tempHeartbeatTime = Mathf.RoundToInt((tempHeartbeatTime / 70.0f)*4);
        if(tempHeartbeatTime != this.heartbeatTime){
            this.heartbeatTime = tempHeartbeatTime;
            device.sendActionMessage("led/heartbeat",this.heartbeatTime.ToString());
            if(this.heartbeatTime > 0){
                this.heartbeat = true;
            }else{
                this.heartbeat = false;
            }
        }
    }

    public int getHeartbeatTime()
    {
        return heartbeatTime;
    }

    public bool getHeartbeatState(){
        return heartbeat;
    }
}
