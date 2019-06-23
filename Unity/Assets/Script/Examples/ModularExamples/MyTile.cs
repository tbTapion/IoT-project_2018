using ExactFramework.Component.Examples;
using ExactFramework.Configuration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTile : TwinObject
{   
    //Set this in the inspector so components can connect, or create subclasses to set specific config named per Twin Object.
    public string configToSet = "";
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        configName = configToSet;
        AddDeviceComponent<RingLight>();
        AddDeviceComponent<IMU>();
        AddDeviceComponent<TonePlayer>();
    }
}
