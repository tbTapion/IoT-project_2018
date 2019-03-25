using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfFlight : DeviceComponent
{
    private int distance;
    private bool measuringRange;

    public TimeOfFlight(TwinObject device){
        this.device = device;
        distance = 9999;
    }
    public override void update () {
	}

    public void setDistance(int distance){
        this.distance = distance;
    }

    public int getDistance(){
        return distance;
    }

    public void setMeasuring(bool measuringRange)
    {
        this.measuringRange = measuringRange;
    }

    public bool getMeasuring(){
        return measuringRange;
    }
}
