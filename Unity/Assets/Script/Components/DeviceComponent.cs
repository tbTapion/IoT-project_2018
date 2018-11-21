using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeviceComponent {

    protected TwinObject device;

    public void setDevice(TwinObject device){
        this.device = device;
    }

    public TwinObject getDevice(){
        return device;
    }

    public virtual void update(){}
}