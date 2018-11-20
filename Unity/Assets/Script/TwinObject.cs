using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TwinObject : MonoBehaviour {

    //MQTT variables
    private MQTTHandler mqttHandler; //Handler for messages
    private string deviceID; //ID of the device
    private bool linked;
    protected string configName;

	// Use this for initialization
	public void Start () {
	}

    public void setMQTTHandler(MQTTHandler mqttHandler)
    {
        this.mqttHandler = mqttHandler;
    }

    public void sendPingMessage(string deviceID)
    {
        mqttHandler.sendDeviceMessage(deviceID + "/ping", "1");
    }

    public void sendActionMessage(string deviceID, string componentName, string payload)
    {
		mqttHandler.sendDeviceMessage(deviceID + "/action/" + componentName, payload);
    }

    public void sendGetMessage(string deviceID, string componentName)
    {
        mqttHandler.sendDeviceMessage(deviceID + "/get/" + componentName, "1");
    }

    public void sendGetConfigMessage(string deviceID)
    {
        mqttHandler.sendDeviceMessage(deviceID + "/getconfg", "1");
    }

    public string getDeviceID()
    {
        return deviceID;
    }

    public bool getLinkStatus()
    {
        return linked;
    }

	public void setLinkStatus(bool linked)
	{
		this.linked = linked;
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
    public virtual void Update()
    {

    }

	public virtual void eventMessage (string[] topic, string payload){
		updateComponent (topic[4], payload);
	}
	public virtual void valueMessage (string[] topic, string payload){
		updateComponent (topic[4], payload);
	}

	protected virtual void updateComponent (string component, string payload)
    {

    }
}
