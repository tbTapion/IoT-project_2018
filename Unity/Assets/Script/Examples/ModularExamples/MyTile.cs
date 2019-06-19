using ExactFramework.Component.Examples;
using ExactFramework.Configuration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTile : TwinObject
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        configName = "tile";
        AddDeviceComponent<RingLight>();
        AddDeviceComponent<IMU>();
        AddDeviceComponent<TonePlayer>();
    }
}
