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
        client.MqttMsgPublishReceived += handleMQTTMessage; //Setting up the function triggered on received messages
        client.Connect("Unity02"); //Connecting to the MQTT broker specified on the ip in the client init - String: client ID
        client.Subscribe(new string[] { "unity/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); //Subscribing to the topic
        client.Publish("esp8266/hello", new byte[] { 0 });
    }
    /*
    Sends a message to the MQTT server. Message built on TwinObject.
     */

    internal void sendDeviceMessage(string tempMessageTopic, byte[] payload)
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
    void handleMQTTMessage(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + e.Topic);
        msgBuffer.Add(new MessagePair(e.Topic, e.Message));
    }

    /*
    Update function called from unity thread to handle messages in the unity thread.
     */
    public void update()
    {
        if (allDevicesConnected())
        {
            foreach (TwinObject obj in twinObjects)
            {
                if (obj.getLinkStatus())
                {
                    if (obj.actionMsgBuffer.Count > 0)
                    {
                        delay = obj.actionMsgBuffer.Count * 50;
                        string actionMessageString = "";
                        List<byte> payloads = new List<byte>();
                        actionMessageString += obj.getDeviceID() + "/action";
                        foreach (MessagePair mp in obj.actionMsgBuffer)
                        {
                            actionMessageString += "/" + mp.topic;
                            payloads.AddRange(mp.payload);
                        }
                        obj.actionMsgBuffer = new List<MessagePair>();
                        sendDeviceMessage(actionMessageString, payloads.ToArray());
                    }
                    if (obj.getMsgBuffer.Count > 0)
                    {
                        delay = obj.getMsgBuffer.Count * 20;
                        string getMessageString = "";
                        getMessageString += obj.getDeviceID() + "/get";
                        foreach (MessagePair mp in obj.getMsgBuffer)
                        {
                            getMessageString += "/" + mp.topic;
                            obj.getMsgBuffer.Remove(mp);
                        }
                        obj.getMsgBuffer = new List<MessagePair>();
                        sendDeviceMessage(getMessageString, new byte[] { 0 });
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
                    deviceConnect(topicSplit);
                }
                else if (topicSplit[1] == "device")
                {
                    if (topicSplit[3] == "event")
                    {
                        deviceEvent(topicSplit, message.payload);
                    }
                    else if (topicSplit[3] == "value")
                    {
                        deviceValue(topicSplit, message.payload);
                    }
                    else if (topicSplit[3] == "ping")
                    {
                        devicePing(topicSplit[2]);
                    }
                }
            }
        }
    }

    private void deviceValue(string[] topicSplit, byte[] payload)
    {
        TwinObject to = getObjectByID(topicSplit[2]);
        if (to != null)
        {
            to.valueMessage(topicSplit, payload);
        }
    }

    private void deviceEvent(string[] topicSplit, byte[] payload)
    {
        TwinObject to = getObjectByID(topicSplit[2]);
        if (to != null)
        {
            to.eventMessage(topicSplit, payload);
        }
    }

    private void devicePing(string deviceID)
    {
        TwinObject to = getObjectByID(deviceID);
        if (to != null)
        {
            if (!to.getLinkStatus())
            {
                to.setLinkStatus(true);
            }
            to.pingResponse();
        }
    }

    private void deviceConnect(string[] topicSplit)
    {
        Debug.Log("New Device detected!");
        bool linkPossible = false;
        foreach (TwinObject obj in twinObjects)
        {
            if (obj.getDeviceID() == topicSplit[2])
            {
                obj.setLinkStatus(true);
                linkPossible = true;
                break;
            }
            else if (obj.getConfigName() == topicSplit[3] && obj.getLinkStatus() == false)
            {
                obj.linkDevice(topicSplit[2]);
                linkPossible = true;
                break;
            }
        }
        if (!linkPossible)
        {
            sendDeviceMessage(topicSplit[2] + "/ping", new byte[] { 0 });
        }
    }

    private TwinObject getObjectByID(string deviceID)
    {
        foreach (TwinObject obj in twinObjects)
        {
            if (obj.getDeviceID() == deviceID)
            {
                return obj;
            }
        }
        return null;
    }

    public List<TwinObject> getTwinObjectList()
    {
        return twinObjects;
    }

    public List<TwinObject> getConnectedTwinObjects()
    {
        List<TwinObject> temp = new List<TwinObject>();
        foreach (TwinObject obj in twinObjects)
        {
            if (obj.getLinkStatus())
            {
                temp.Add(obj);
            }
        }
        return temp;
    }

    public void addTwinObject(TwinObject obj)
    {
        obj.setMQTTHandler(this);
        twinObjects.Add(obj);
    }

    public Boolean allDevicesConnected()
    {
        bool connected = true;
        foreach (TwinObject obj in twinObjects)
        {
            if (obj.getLinkStatus() == false)
            {
                connected = false;
                break;
            }
        }
        return connected;
    }
}