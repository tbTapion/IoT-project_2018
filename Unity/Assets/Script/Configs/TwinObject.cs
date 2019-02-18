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
    protected string configName; // Name of the config type. Set in inherited classes

    //Ping variables used to measure ping. Ping count reaches 2 and device is deemed disconnected.
    private int pingCount;
    private int pingTime;

	// Use this for initialization
	public virtual void Start () {
        pingCount = 0;
        pingTime = 30*60; // How much time to lapse before ping count increased, and how often ping is happening. Fix later for better time management.
	}
    
    // Update is called once per frame
    public virtual void Update()
    {   
        //Ping checks and message sent
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

    //Sets the mqtt-handler reference. Used in message sending methods.
    public void setMQTTHandler(MQTTHandler mqttHandler)
    {
        this.mqttHandler = mqttHandler;
    }

    //Builds the ping message and sends it with the mqtt-handlers send method.
    public void sendPingMessage()
    {
        mqttHandler.sendDeviceMessage(deviceID + "/ping", "1");
    }

    //Builds the action message and sends it with the mqtt-handlers send method.
    public void sendActionMessage(string componentName, string payload)
    {
		mqttHandler.sendDeviceMessage(deviceID + "/action/" + componentName, payload);
    }

    //Builds the get message and sends it with the mqtt-handlers send method.
    public void sendGetMessage(string componentName)
    {
        mqttHandler.sendDeviceMessage(deviceID + "/get/" + componentName, "1");
    }

    //Builds the get config message and sends it with the mqtt-handlers send method. *** unused ***
    public void sendGetConfigMessage()
    {
        mqttHandler.sendDeviceMessage(deviceID + "/getconfg", "1");
    }

    //Gets the ID, arduino mac adress, of device linked for this TwinObject.
    public string getDeviceID()
    {
        return deviceID;
    }

    //Returns whether the TwinObject has a link established with a device (and object) or not.
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

    //Method to set the link status. Primarily used by the mqtt-handler.
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
