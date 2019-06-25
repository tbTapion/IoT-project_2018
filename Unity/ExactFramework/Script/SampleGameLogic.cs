using System.Collections;
using System.Collections.Generic;
using ExactFramework.Handlers;
using UnityEngine;

public class SampleGameLogic : MonoBehaviour
{
    private MQTTHandler mqttHandler;

    // Start is called before the first frame update
    void Start()
    {
        //MQTT handler. Takes care of the connection to the RPI and sending/receiving messages.
        mqttHandler = new MQTTHandler("129.241.104.251"); //Enter IP here
    }

    // Update is called once per frame
    void Update()
    {
        mqttHandler.Update(); //MQTT handler's update function. Handles updating all the objects based on incoming messages.
        if (mqttHandler.AllDevicesConnected()) //Checks to see if all devices are connected.
        {
            Debug.Log("Main Update of Logic class");
        }
    }
}
