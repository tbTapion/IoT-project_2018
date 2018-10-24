using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class Test : MonoBehaviour {
    private MqttClient client;

	// Use this for initialization
	void Start () {
        client = new MqttClient(IPAddress.Parse("192.168.42.1"), 1884, false, null);

        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        client.Connect("testmqttunity");

        client.Subscribe(new string[] { "hello/world" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
