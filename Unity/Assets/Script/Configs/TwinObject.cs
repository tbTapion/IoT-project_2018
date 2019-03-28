using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TwinObject : MonoBehaviour
{

    //MQTT variables
    private MQTTHandler mqttHandler; //Handler for messages
    public string deviceID; //ID of the device
    public bool linked; //Link bool for the link between Digital Twin Object(this) and physical device
    protected string configName;

    private int pingCount;
    private int pingTime;

    private string name;

    public List<MessagePair> actionMsgBuffer = new List<MessagePair>();
    public List<MessagePair> getMsgBuffer = new List<MessagePair>();

    // Use this for initialization
    private void Start()
    {
        pingCount = 0;
        pingTime = 60 * 60;
    }

    // Update is called once per frame
    private void Update()
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
                }
                else
                {
                    SendPingMessage();
                    pingCount++;
                }
                pingTime = 60 * 60;
            }
        }
    }

    /*
    Sets the mqttHandler object-link in the class. 
     */
    public void SetMQTTHandler(MQTTHandler mqttHandler)
    {
        this.mqttHandler = mqttHandler;
    }
    
    //Sends the device message. Takes topic string and string payload. Converts payload to bytes.
    internal void SendDeviceMessage(string tempMessageTopic, string payload)
    {
        if (mqttHandler != null)
        {
            mqttHandler.SendDeviceMessage(tempMessageTopic, System.Text.Encoding.Default.GetBytes(payload));
        }
    }

    internal void SendDeviceMessage(string tempMessageTopic, int payload)
    {
        if (mqttHandler != null)
        {
            mqttHandler.SendDeviceMessage(tempMessageTopic, new byte[] {(byte)payload });
        }
    }

    internal void SendDeviceMessage(string tempMessageTopic, int[] payload)
    {

    }

    internal void SendDeviceMessage(string tempMessageTopic, bool payload)
    {
        if (mqttHandler != null)
        {
            if(payload == true){
                mqttHandler.SendDeviceMessage(tempMessageTopic, new byte[] {(byte)1});
            }else{
                mqttHandler.SendDeviceMessage(tempMessageTopic, new byte[] {(byte)0});
            }
        }
    }

    public void SendActionMessage(string componentName, string payload)
    {
        //sendDeviceMessage(deviceID + "/action/" + componentName, payload);
        List<byte> temp = new List<byte>();
        temp.Add((byte)payload.Length);
        temp.AddRange(System.Text.Encoding.Default.GetBytes(payload));
        AddActionMessageToBuffer(componentName, temp.ToArray());
    }

    public void SendActionMessage(string componentName, int payload)
    {
        //sendDeviceMessage(deviceID + "/action/" + componentName, payload);
        AddActionMessageToBuffer(componentName, new byte[] {1, (byte)payload });
    }

    public void SendActionMessage(string componentName, bool payload)
    {
        if (mqttHandler != null)
        {
            if(payload == true){
                AddActionMessageToBuffer(componentName, new byte[] {1, (byte)1});
            }else{
                AddActionMessageToBuffer(componentName, new byte[] {1,  (byte)0});
            }
        }
        //sendDeviceMessage(deviceID + "/action/" + componentName, payload);
    }

    public void SendActionMessage(string componentName, byte[] payload)
    {
        if (mqttHandler != null)
        {
            //mqttHandler.sendDeviceMessage(deviceID + "/action/" + componentName, payload);
            List<byte> temp = new List<byte>();
            temp.Add((byte)payload.Length);
            temp.AddRange(payload);
            AddActionMessageToBuffer(componentName, temp.ToArray());
        }
    }

    private void AddActionMessageToBuffer(string componentName, byte[] payload){
        actionMsgBuffer.Add(new MessagePair(componentName, payload));
    }

    public void SendPingMessage()
    {
        SendDeviceMessage(deviceID + "/ping", true);
    }

    public void SendGetMessage(string componentName)
    {
        //sendDeviceMessage(deviceID + "/get/" + componentName, 1);
        if(mqttHandler != null){
            getMsgBuffer.Add(new MessagePair(componentName, new byte[]{0}));
        }
    }

    public void SendGetConfigMessage()
    {
        SendDeviceMessage(deviceID + "/getconfg", 1);
    }

    public string GetDeviceID()
    {
        return deviceID;
    }

    public bool GetLinkStatus()
    {
        return linked;
    }

    public void SetLinkStatus(bool linked)
    {
        this.linked = linked;
    }

    public void LinkDevice(string deviceID)
    {
        this.deviceID = deviceID;
        SetLinkStatus(true);
        SendPingMessage();
    }

    public string GetConfigName()
    {
        return configName;
    }

    public virtual void EventMessage(string[] topic, byte[] payload)
    {   
        pingTime = 60*60;
        EventMessage msg = new EventMessage(topic[4], topic[5], payload);
        UpdateComponent(msg);
        OnEvent(msg);
    }

    public virtual void ValueMessage(string[] topic, byte[] payload)
    {
        pingTime = 60*60;
        EventMessage msg = new EventMessage(topic[4], topic[5], payload);
        UpdateComponent(new EventMessage(topic[4], topic[5], payload));
    }

    public void PingResponse()
    {
        pingCount = 0;
    }

    public void SetName(string name){
        if(transform != null){
            transform.name = name;
        }
        this.name = name;
    }

    protected virtual void OnEvent(EventMessage e) { }

    protected abstract void UpdateComponent(EventMessage e);
}
