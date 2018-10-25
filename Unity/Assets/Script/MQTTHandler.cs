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
    private GameLogic gameLogic;

	// Use this for initialization
	public MQTTHandler (GameLogic gameLogic, string hostaddress="127.0.0.1", int port=1883) {
        this.gameLogic = gameLogic;
        client = new MqttClient(IPAddress.Parse(hostaddress), port, false, null); //Initializing the MQTT Class with ip, port, SSL level.

        client.MqttMsgPublishReceived += handleMQTTMessage; //Setting up the function triggered on received messages

        client.Connect("Unity01"); //Connecting to the MQTT broker specified on the ip in the client init - String: client ID

        client.Subscribe(new string[] {"unity/#"}, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); //Subscribing to the topic
        client.Publish("unity/hellowoworld", Encoding.UTF8.GetBytes("Hello World")); //test message on topic
    }

    internal void sendDeviceMessage(string tempMessageTopic, int payload)
    {
        client.Publish(tempMessageTopic, Encoding.UTF8.GetBytes(payload.ToString()));
    }

    void handleMQTTMessage(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + Encoding.UTF8.GetString(e.Message));
        String topic = e.Topic;
        String[] topicSplit = topic.Split('/');
        if(topicSplit[0] == "unity")
        {
            if (topicSplit[1] == "connect")
            {
                deviceConnect(topicSplit);
            }else if(topicSplit[1] == "device")
            {
                if (topicSplit[3] == "event")
                {
                    deviceEvent(topicSplit);
                }
                else if (topicSplit[3] == "value")
                {
                    deviceValue(topicSplit);
                }
            }
        }
    }

    private void deviceValue(string[] topicSplit)
    {

    }

    private void deviceEvent(string[] topicSplit)
    {

    }

    private void deviceConnect(string[] topicSplit)
    {
        bool linkListFull = true;
        List<GameObject> objectList = gameLogic.getTwinObjectList();
        foreach (GameObject obj in objectList)
        {
            if (!obj.GetComponent<TwinObject>().getLinkStatus())
            {
                obj.GetComponent<TwinObject>().linkDevice(topicSplit[2]);
;                linkListFull = false;
                break;
            }
        }
        if (linkListFull)
        {
            sendDeviceMessage(topicSplit[2] + "/ping", 0);
        }
    }
}