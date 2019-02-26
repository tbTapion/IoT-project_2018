using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TwinObject : MonoBehaviour {

    //MQTT variables
    private MQTTHandler mqttHandler; //Handler for messages
    private string deviceID; //ID of the device
    private bool linked; //Linked to both object and device
    private bool deviceLink; // Linked to device
    private bool objectLink; // Linked to object
    protected string configName;

    private int pingCount;
    private int pingTime;

	// Use this for initialization
	public virtual void Start() {
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
                    //deviceLink = false;
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

    public bool getDeviceLinkStatus()
    {
        return deviceLink;
    }

    public bool getObjectLinkStatus()
    {
        return objectLink;
    }

	public void setLinkStatus(bool linked)
	{
		this.linked = linked;
	}

    public void setDeviceLinkStatus(bool deviceLink)
    {
        this.deviceLink = deviceLink;
    }

    public void setObjectLinkStatus(bool objectLink)
    {
        this.objectLink = objectLink;
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
        EventMessage msg = new EventMessage(topic[4], payload);
		updateComponent (msg);
        onEvent(msg);
	}
	public virtual void valueMessage (string[] topic, string payload){
        EventMessage msg = new EventMessage(topic[4], payload);
		updateComponent (new EventMessage(topic[4], payload));
	}

    public void pingResponse(){
        pingCount = 0;
    }

    protected virtual void onEvent (EventMessage e){}

	protected abstract void updateComponent (EventMessage e);
}
