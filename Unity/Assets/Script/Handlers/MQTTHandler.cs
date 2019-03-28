using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;

public class MQTTHandler
{

    //MQTT Client class
    private MqttClient client;
    private List<TwinObject> twinObjects = new List<TwinObject>();
    private List<MessagePair> msgBuffer = new List<MessagePair>();

    private int delay = 0;

    /*
    Initialization of the MQTTHandler object.
     */
    public MQTTHandler(string hostaddress = "127.0.0.1", int port = 1883)
    {
        client = new MqttClient(IPAddress.Parse(hostaddress), port, false, null); //Initializing the MQTT Class with ip, port, SSL level.
        client.MqttMsgPublishReceived += HandleMQTTMessage; //Setting up the function triggered on received messages
        client.Connect("Unity02"); //Connecting to the MQTT broker specified on the ip in the client init - String: client ID
        client.Subscribe(new string[] { "unity/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); //Subscribing to the topic
        client.Publish("esp8266/hello", new byte[] { 0 });
    }
    /*
    Sends a message to the MQTT server. Message built on TwinObject.
     */

    internal void SendDeviceMessage(string tempMessageTopic, byte[] payload)
    {
        client.Publish(tempMessageTopic, payload);
        if (delay != 0)
        {
            System.Threading.Thread.Sleep(delay);
            delay = 0;
        }
    }

    /*
    MQTT message handler. Handles incoming MQTT messages and creates a message pair object with the topic and payload. The message pair is added to the msgBuffer object which is called in the standard unity thread for messages to be further handled.
     */
    void HandleMQTTMessage(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + e.Topic);
        msgBuffer.Add(new MessagePair(e.Topic, e.Message));
    }

    /*
    Update function called from unity thread to handle messages in the unity thread.
     */
    public void Update()
    {
        if (AllDevicesConnected())
        {
            foreach (TwinObject obj in twinObjects)
            {
                if (obj.GetLinkStatus())
                {
                    if (obj.actionMsgBuffer.Count > 0)
                    {
                        delay = obj.actionMsgBuffer.Count * 50;
                        string actionMessageString = "";
                        List<byte> payloads = new List<byte>();
                        actionMessageString += obj.GetDeviceID() + "/action";
                        foreach (MessagePair mp in obj.actionMsgBuffer)
                        {
                            actionMessageString += "/" + mp.topic;
                            payloads.AddRange(mp.payload);
                        }
                        obj.actionMsgBuffer = new List<MessagePair>();
                        SendDeviceMessage(actionMessageString, payloads.ToArray());
                    }
                    if (obj.getMsgBuffer.Count > 0)
                    {
                        delay = obj.getMsgBuffer.Count * 20;
                        string getMessageString = "";
                        getMessageString += obj.GetDeviceID() + "/get";
                        foreach (MessagePair mp in obj.getMsgBuffer)
                        {
                            getMessageString += "/" + mp.topic;
                            obj.getMsgBuffer.Remove(mp);
                        }
                        obj.getMsgBuffer = new List<MessagePair>();
                        SendDeviceMessage(getMessageString, new byte[] { 0 });
                    }
                }
            }
        }

        while (msgBuffer.Count != 0)
        {
            MessagePair message = msgBuffer[0];
            msgBuffer.RemoveAt(0);
            String[] topicSplit = message.topic.Split('/');
            if (topicSplit[0] == "unity")
            {
                if (topicSplit[1] == "connect")
                {
                    DeviceConnect(topicSplit);
                }
                else if (topicSplit[1] == "device")
                {
                    if (topicSplit[3] == "event")
                    {
                        DeviceEvent(topicSplit, message.payload);
                    }
                    else if (topicSplit[3] == "value")
                    {
                        DeviceValue(topicSplit, message.payload);
                    }
                    else if (topicSplit[3] == "ping")
                    {
                        DevicePing(topicSplit[2]);
                    }
                }
            }
        }
    }

    private void DeviceValue(string[] topicSplit, byte[] payload)
    {
        TwinObject to = GetObjectByID(topicSplit[2]);
        if (to != null)
        {
            to.ValueMessage(topicSplit, payload);
        }
    }

    private void DeviceEvent(string[] topicSplit, byte[] payload)
    {
        TwinObject to = GetObjectByID(topicSplit[2]);
        if (to != null)
        {
            to.EventMessage(topicSplit, payload);
        }
    }

    private void DevicePing(string deviceID)
    {
        TwinObject to = GetObjectByID(deviceID);
        if (to != null)
        {
            if (!to.GetLinkStatus())
            {
                to.SetLinkStatus(true);
            }
            to.PingResponse();
        }
    }

    private void DeviceConnect(string[] topicSplit)
    {
        Debug.Log("New Device detected!");
        bool linkPossible = false;
        foreach (TwinObject obj in twinObjects)
        {
            if (obj.GetDeviceID() == topicSplit[2])
            {
                obj.SetLinkStatus(true);
                linkPossible = true;
                break;
            }
            else if (obj.GetConfigName() == topicSplit[3] && obj.GetLinkStatus() == false)
            {
                obj.LinkDevice(topicSplit[2]);
                linkPossible = true;
                break;
            }
        }
        if (!linkPossible)
        {
            SendDeviceMessage(topicSplit[2] + "/ping", new byte[] { 0 });
        }
    }

    private TwinObject GetObjectByID(string deviceID)
    {
        foreach (TwinObject obj in twinObjects)
        {
            if (obj.GetDeviceID() == deviceID)
            {
                return obj;
            }
        }
        return null;
    }

    public List<TwinObject> GetTwinObjectList()
    {
        return twinObjects;
    }

    public List<TwinObject> GetConnectedTwinObjects()
    {
        List<TwinObject> temp = new List<TwinObject>();
        foreach (TwinObject obj in twinObjects)
        {
            if (obj.GetLinkStatus())
            {
                temp.Add(obj);
            }
        }
        return temp;
    }

    public void AddTwinObject(TwinObject obj)
    {
        obj.SetMQTTHandler(this);
        twinObjects.Add(obj);
    }

    public bool AllDevicesConnected()
    {
        bool connected = true;
        foreach (TwinObject obj in twinObjects)
        {
            if (obj.GetLinkStatus() == false)
            {
                connected = false;
                break;
            }
        }
        return connected;
    }
}