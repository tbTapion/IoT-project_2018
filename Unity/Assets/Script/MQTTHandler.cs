using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;

public class MQTTHandler{

    //MQTT Client class
    private MqttClient client;
    private List<TwinObject> twinObjects = new List<TwinObject>();
    private GameLogic gameLogic;
    private List<MessagePair> msgBuffer = new List<MessagePair>();

    // Use this for initialization
    public MQTTHandler (GameLogic gameLogic, string hostaddress="127.0.0.1", int port=1883) {
        this.gameLogic = gameLogic;
        client = new MqttClient(IPAddress.Parse(hostaddress), port, false, null); //Initializing the MQTT Class with ip, port, SSL level.

        client.MqttMsgPublishReceived += handleMQTTMessage; //Setting up the function triggered on received messages

        client.Connect("Unity01"); //Connecting to the MQTT broker specified on the ip in the client init - String: client ID

        client.Subscribe(new string[] {"unity/#"}, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); //Subscribing to the topic
        //client.Publish("unity/hellowoworld", Encoding.UTF8.GetBytes("Hello World")); //test message on topic
    }

    internal void sendDeviceMessage(string tempMessageTopic, string payload)
    {
        client.Publish(tempMessageTopic, Encoding.UTF8.GetBytes(payload));
    }

    void handleMQTTMessage(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + e.Topic);
        msgBuffer.Add(new MessagePair(e.Topic, Encoding.UTF8.GetString(e.Message)));
    }

    public void update()
    {
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

	private void deviceValue(string[] topicSplit, string payload)
    {
		TwinObject to = getObjectByID (topicSplit [2]);
		if (to) {
			to.valueMessage (topicSplit, payload);
		}
    }

	private void deviceEvent(string[] topicSplit, string payload)
    {
		TwinObject to = getObjectByID (topicSplit [2]);
		if (to) {
			to.eventMessage (topicSplit, payload);
		}
    }

	private void devicePing(string deviceID)
	{	
		TwinObject to = getObjectByID (deviceID);
		if (to) {
            if(!to.getLinkStatus()){
                to.setLinkStatus (true);
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
            if (obj.getConfigName() == topicSplit[3] && obj.getLinkStatus() == false)
            {
                obj.linkDevice(topicSplit[2]);
                linkPossible = true;
                break;
            }
        }
        if (!linkPossible)
        {
            sendDeviceMessage(topicSplit[2] + "/ping", "0");
        }
    }

	private TwinObject getObjectByID(string deviceID){
		foreach (TwinObject obj in twinObjects) {
			if (obj.getDeviceID() == deviceID) {
				return obj;
			}
		}
		return null;
	}

    public List<TwinObject> getTwinObjectList()
    {
        return twinObjects;
    }

    public void addTwinObject(TwinObject obj)
    {
        twinObjects.Add(obj);
    }
}