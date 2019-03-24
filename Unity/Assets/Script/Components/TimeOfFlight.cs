using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfFlight : DeviceComponent
{
    public int distance;

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
}
