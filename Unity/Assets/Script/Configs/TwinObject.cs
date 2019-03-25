using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TwinObject : MonoBehaviour
{

    //MQTT variables
    private MQTTHandler mqttHandler; //Handler for messages
    public string deviceID; //ID of the device
    public bool linked; //Linked to both object and device
    private bool deviceLink; // Linked to device
    private bool objectLink; // Linked to object
    protected string configName;

    private int pingCount;
    private int pingTime;

    private string name;

    public List<MessagePair> actionMsgBuffer = new List<MessagePair>();
    public List<MessagePair> getMsgBuffer = new List<MessagePair>();

    // Use this for initialization
    public virtual void Start()
    {
        pingCount = 0;
        pingTime = 60 * 60;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (linked)
        {
            pingTime--;
            if (pingTime == 0)
            {
                if (pingCount > 2)
                {
                    linked = false;
                    Debug.Log("Tile " + deviceID + " disconnected...");
                    deviceLink = false;
                }
                else
                {
                    sendPingMessage();
                    pingCount++;
                }
                pingTime = 60 * 60;
            }
        }
    }

    /*
    Sets the mqttHandler object-link in the class. 
     */
    public void setMQTTHandler(MQTTHandler mqttHandler)
    {
        this.mqttHandler = mqttHandler;
    }
    
    //Sends the device message. Takes topic string and string payload. Converts payload to bytes.
    internal void sendDeviceMessage(string tempMessageTopic, string payload)
    {
        if (mqttHandler != null)
        {
            mqttHandler.sendDeviceMessage(tempMessageTopic, System.Text.Encoding.Default.GetBytes(payload));
        }
    }

    internal void sendDeviceMessage(string tempMessageTopic, int payload)
    {
        if (mqttHandler != null)
        {
            mqttHandler.sendDeviceMessage(tempMessageTopic, new byte[] {(byte)payload });
        }
    }

    internal void sendDeviceMessage(string tempMessageTopic, int[] payload)
    {

    }

    internal void sendDeviceMessage(string tempMessageTopic, bool payload)
    {
        if (mqttHandler != null)
        {
            if(payload == true){
                mqttHandler.sendDeviceMessage(tempMessageTopic, new byte[] {(byte)1});
            }else{
                mqttHandler.sendDeviceMessage(tempMessageTopic, new byte[] {(byte)0});
            }
        }
    }

    public void sendActionMessage(string componentName, string payload)
    {
        //sendDeviceMessage(deviceID + "/action/" + componentName, payload);
        List<byte> temp = new List<byte>();
        temp.Add((byte)payload.Length);
        temp.AddRange(System.Text.Encoding.Default.GetBytes(payload));
        addActionMessageToBuffer(componentName, temp.ToArray());
    }

    public void sendActionMessage(string componentName, int payload)
    {
        //sendDeviceMessage(deviceID + "/action/" + componentName, payload);
        addActionMessageToBuffer(componentName, new byte[] {1, (byte)payload });
    }

    public void sendActionMessage(string componentName, bool payload)
    {
        if (mqttHandler != null)
        {
            if(payload == true){
                addActionMessageToBuffer(componentName, new byte[] {1, (byte)1});
            }else{
                addActionMessageToBuffer(componentName, new byte[] {1,  (byte)0});
            }
        }
        //sendDeviceMessage(deviceID + "/action/" + componentName, payload);
    }

    public void sendActionMessage(string componentName, byte[] payload)
    {
        if (mqttHandler != null)
        {
            //mqttHandler.sendDeviceMessage(deviceID + "/action/" + componentName, payload);
            List<byte> temp = new List<byte>();
            temp.Add((byte)payload.Length);
            temp.AddRange(payload);
            addActionMessageToBuffer(componentName, temp.ToArray());
        }
    }

    private void addActionMessageToBuffer(string componentName, byte[] payload){
        actionMsgBuffer.Add(new MessagePair(componentName, payload));
    }

    public void sendPingMessage()
    {
        sendDeviceMessage(deviceID + "/ping", true);
    }

    public void sendGetMessage(string componentName)
    {
        //sendDeviceMessage(deviceID + "/get/" + componentName, 1);
        if(mqttHandler != null){
            getMsgBuffer.Add(new MessagePair(componentName, new byte[]{0}));
        }
    }

    public void sendGetConfigMessage()
    {
        sendDeviceMessage(deviceID + "/getconfg", 1);
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

    public virtual void eventMessage(string[] topic, byte[] payload)
    {   
        pingTime = 60*60;
        EventMessage msg = new EventMessage(topic[4], topic[5], payload);
        updateComponent(msg);
        onEvent(msg);
    }
    public virtual void valueMessage(string[] topic, byte[] payload)
    {
        pingTime = 60*60;
        EventMessage msg = new EventMessage(topic[4], topic[5], payload);
        updateComponent(new EventMessage(topic[4], topic[5], payload));
    }

    public void pingResponse()
    {
        pingCount = 0;
    }

    public void setName(string name){
        if(transform != null){
            transform.name = name;
        }
        this.name = name;
    }

    protected virtual void onEvent(EventMessage e) { }

    protected abstract void updateComponent(EventMessage e);
}
