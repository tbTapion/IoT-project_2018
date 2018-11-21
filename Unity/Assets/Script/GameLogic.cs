using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    private MQTTHandler mqttHandler;

    public GameObject obj;

    GameObject cube1, cube2;

	// Use this for initialization
	void Start () {
        mqttHandler = new MQTTHandler(this, "192.168.42.1");

        cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube1.AddComponent<Cube1>();
        cube1.GetComponent<Cube1>().setMQTTHandler(mqttHandler);
        cube1.transform.position = new Vector3(-0.8f,0f,0f);
        mqttHandler.addTwinObject(cube1.GetComponent<TwinObject>());

        cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.AddComponent<Cube2>();
        cube2.GetComponent<Cube2>().setMQTTHandler(mqttHandler);
        cube2.transform.position = new Vector3(0.8f, 0f, 0f);
        mqttHandler.addTwinObject(cube2.GetComponent<TwinObject>());
    }

	// Update is called once per frame
	void Update () {
        mqttHandler.update();
        if (cube2.GetComponent<Cube2>().getButton().justPressed())
        {
            cube1.GetComponent<Cube1>().getLed().toggle();
        }
        cube1.GetComponent<Cube1>().getLed().setHeartbeatTime(cube2.GetComponent<Cube2>().getPotmeter().getValue());
	}
}
