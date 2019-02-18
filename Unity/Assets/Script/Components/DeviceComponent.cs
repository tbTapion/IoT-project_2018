using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeviceComponent {

    protected TwinObject device; // Device the component is on

    /*
    Sets the device the component is on. */
    public void setDevice(TwinObject device){
        this.device = device;
    }

    /*
    Gets the device the component is on. */
    public TwinObject getDevice(){
        return device;
    }

    /*Abstract update method to call in the twin object's update method. */
    public abstract void update();
}