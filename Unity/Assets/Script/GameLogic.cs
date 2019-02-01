using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    private MQTTHandler mqttHandler;

    List<GameObject> objectList = new List<GameObject>();


    GameObject activatedObject;

    int numberOfObjects = 3;

	// Use this for initialization
	void Start () {
        //MQTT handler. Takes care of the connection to the RPI and sending/receiving messages.
        mqttHandler = new MQTTHandler(this, "192.168.42.1");

        //Test object
        for(int i = 0; i<numberOfObjects; i++){
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = new Vector3(-1.6f + (i*1.05f), 0.0f, 0.0f);
            objectList.Add(obj);
            mqttHandler.addTwinObject(obj.AddComponent<Cube3>());
        }
    }

	// Update is called once per frame
	void Update () {
        mqttHandler.update();
        if(mqttHandler.allDevicesConnected()){
            if(activatedObject == null){
                activatedObject = objectList[Random.Range(0, objectList.Count-1)];
                activatedObject.GetComponent<Cube3>().getLed().setState(true);
            }else if(activatedObject.GetComponent<Cube3>().getButton().justPressed()){
                activatedObject.GetComponent<Cube3>().getLed().setState(false);
                activatedObject = null;
            }
        }
	}
}
