using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TwinObject : MonoBehaviour {

    //MQTT variables
    private MQTTHandler mqttHandler; //Handler for messages
    private string deviceID; //ID of the device
    private bool linked;
    protected string configName;

    private int pingCount;
    private int pingTime;

	// Use this for initialization
	public virtual void Start () {
        pingCount = 0;
        pingTime = 30*60;
	}
    
    // Update is called once per frame
    public virtual void Update()
    {
        if(linked){
            pingTime--;
            if(pingTime == 0){
                if(pingCount > 2){
                    linked = false;
                }else{
                    sendPingMessage();
                    pingCount++;
                }
                pingTime = 30*60;
            }
        }
    }

    public void setMQTTHandler(MQTTHandler mqttHandler)
    {
        this.mqttHandler = mqttHandler;
    }

    public void sendPingMessage()
    {
        mqttHandler.sendDeviceMessage(deviceID + "/ping", "1");
    }

    public void sendActionMessage(string componentName, string payload)
    {
		mqttHandler.sendDeviceMessage(deviceID + "/action/" + componentName, payload);
    }

    public void sendGetMessage(string componentName)
    {
        mqttHandler.sendDeviceMessage(deviceID + "/get/" + componentName, "1");
    }

    public void sendGetConfigMessage()
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
        sendPingMessage();
    }

    public string getConfigName()
    {
        return configName;
    }

	public virtual void eventMessage (string[] topic, string payload){
		updateComponent (topic[4], payload);
	}
	public virtual void valueMessage (string[] topic, string payload){
		updateComponent (topic[4], payload);
	}

    public void pingResponse(){
        pingCount = 0;
    }

	protected abstract void updateComponent (string component, string payload);
}
