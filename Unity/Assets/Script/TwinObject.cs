using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinObject : MonoBehaviour {

    //MQTT variables
    private MQTTHandler mqttHandler; //Handler for messages
    private string deviceID; //ID of the device
    private bool linked;

    //Variables for the Cube values. Twin object values
    private int led;
    private bool buttonHeld;

	// Use this for initialization
	void Start () {
	}

    void setLedValue(int led)
    {
        string tempMessage = deviceID + "/LED"; //change for proper message transaction
        mqttHandler.sendDeviceMessage(tempMessage, led);
    }

    public string getDeviceID()
    {
        return deviceID;
    }

    public bool getLinkStatus()
    {
        return linked;
    }

    public void linkDevice(string deviceID)
    {
        this.deviceID = deviceID;
        this.linked = true;
        mqttHandler.sendDeviceMessage(deviceID + "/ping", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
