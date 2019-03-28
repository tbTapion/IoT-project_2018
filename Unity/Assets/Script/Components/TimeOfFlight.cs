using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfFlight : DeviceComponent
{
    private int distance;
    private bool measuringDistance;

    private void Start()
    {
        distance = 9999;
    }

    public void SetDistance(int distance){
        this.distance = distance;
    }

    public int GetDistance(){
        return distance;
    }

    public void SetMeasuringDistance(bool measuringDistance)
    {
        this.measuringDistance = measuringDistance;
        if (measuringDistance)
        {
            SendMessage("OnMeasuredDistance");
        }
    }

    public bool GetMeasuring(){
        return measuringDistance;
    }
}
