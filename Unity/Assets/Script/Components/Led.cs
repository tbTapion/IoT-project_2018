﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Led : DeviceComponent {

    protected bool state;
    protected bool heartbeat;
    protected int heartbeatTime;

    private void Start()
    {
        state = false;
    }

    public void Toggle()
    {
        SetState(!state);
    }

    public void SetState(bool state)
    {
        this.state = state;
        device.SendActionMessage("led", (state ? 1 : 0).ToString());
    }

    public bool GetState(){
        return state;
    }

    public void SetHeartbeatTime(int heartbeatTime)
    {
        int tempHeartbeatTime = Mathf.Min(heartbeatTime, 70);
        tempHeartbeatTime = Mathf.RoundToInt((tempHeartbeatTime / 70.0f)*4);
        if(tempHeartbeatTime != this.heartbeatTime){
            this.heartbeatTime = tempHeartbeatTime;
            device.SendActionMessage("led/heartbeat",this.heartbeatTime.ToString());
            if(this.heartbeatTime > 0){
                this.heartbeat = true;
            }else{
                this.heartbeat = false;
            }
        }
    }

    public int GetHeartbeatTime()
    {
        return heartbeatTime;
    }

    public bool GetHeartbeatState(){
        return heartbeat;
    }
}
