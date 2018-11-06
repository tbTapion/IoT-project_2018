using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    private MQTTHandler mqttHandler;

    public GameObject obj;

	// Use this for initialization
	void Start () {
        //mqttHandler = new MQTTHandler(this);
        GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube1.AddComponent<Cube1>();
        cube1.transform.position = new Vector3(-0.8f,0f,0f);
        //mqttHandler.addTwinObject(cube1);
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.AddComponent<Cube2>();
        cube2.transform.position = new Vector3(0.8f, 0f, 0f);
        //mqttHandler.addTwinObject(cube2);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
