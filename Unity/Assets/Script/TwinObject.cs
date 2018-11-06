using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinObject : MonoBehaviour {

    //MQTT variables
    private MQTTHandler mqttHandler; //Handler for messages
    private string deviceID; //ID of the device
    private bool linked;
    protected string configName;

	// Use this for initialization
	void Start () {
	}

    public void sendPingMessage(string deviceID)
    {
        mqttHandler.sendDeviceMessage(deviceID + "/ping", 1);
    }

    public void sendActionMessage(string deviceID, string componentName)
    {
        mqttHandler.sendDeviceMessage(deviceID + "/ping", 1);
    }

    public void sendGetMessage(string deviceID, string componentName)
    {
        mqttHandler.sendDeviceMessage(deviceID + "/get/" + componentName, 1);
    }

    public void sendGetConfigMessage(string deviceID)
    {
        mqttHandler.sendDeviceMessage(deviceID + "/getconfg", 1);
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
        sendPingMessage(deviceID);
    }

    public string getConfigName()
    {
        return configName;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
