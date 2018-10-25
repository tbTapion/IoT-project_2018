using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    private MQTTHandler mqttHandler;
    private List<GameObject> twinObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
        mqttHandler = new MQTTHandler(this);
	}

	// Update is called once per frame
	void Update () {
		
	}

    public List<GameObject> getTwinObjectList()
    {
        return twinObjects;
    }

    public void createTwinObject(string v)
    {
        throw new NotImplementedException();
    }
}
