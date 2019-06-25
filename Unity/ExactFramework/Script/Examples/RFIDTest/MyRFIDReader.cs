using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using ExactFramework.Configuration;
using UnityEngine;

///<summary>
///Fake RFID reader test with an arduino sending a message every 30 seconds on the RFID event
///Following the ModularExamples style and using the GameLogicBase from those examples for the MQTT Handler.
///Tested with a redtile arduino
///</summary>
public class MyRFIDReader : TwinObject
{
    // Start is called before the first frame update
    protected override void Start()
    {  
       base.Start();
       configName = "redtile"; 
    }

    void Awake(){
        AddDeviceComponent<RFID>();
    }
}
