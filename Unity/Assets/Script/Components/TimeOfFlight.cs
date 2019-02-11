using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfFlight : DeviceComponent
{
    private int distance;

    public TimeOfFlight(TwinObject device){
        this.device = device;
        distance = 0;
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
