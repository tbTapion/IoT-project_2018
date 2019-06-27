using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using ExactFramework.Configuration;
using UnityEngine;

public class ButtonLight : TwinObject
{

    void Awake(){
        AddDeviceComponent<Led>();
        AddDeviceComponent<Button>();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        configName = "ButtonLight";
    }
}
