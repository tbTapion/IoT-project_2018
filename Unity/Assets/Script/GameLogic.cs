using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System.Threading;

public class GameLogic : MonoBehaviour {

    private MQTTHandler mqttHandler;

    public List<GameObject> objectList = new List<GameObject>();


    public GameObject activatedObject;

    int numberOfObjects = 4;

	// Use this for initialization
	void Start () {
        //MQTT handler. Takes care of the connection to the RPI and sending/receiving messages.
        mqttHandler = new MQTTHandler("129.241.105.187");

        //Test object
        for(int i = 0; i<numberOfObjects; i++){
            GameObject obj = Instantiate(Resources.Load("Prefabs/Cube3"),new Vector3(-1.6f + (i*1.05f), 0.0f, 0.0f),Quaternion.identity) as GameObject;
            objectList.Add(obj);
            mqttHandler.addTwinObject(obj.GetComponent<Cube3>());
        }
    }

	// Update is called once per frame
	void Update () {
        mqttHandler.update();
        if(mqttHandler.allDevicesConnected()){
            if(activatedObject == null){
                activatedObject = objectList[Random.Range(0, objectList.Count)];
                Thread.Sleep(1000);
                activatedObject.GetComponent<Cube3>().getLed().setState(true);
            }else if(activatedObject.GetComponent<Cube3>().getButton().justPressed()){
                activatedObject.GetComponent<Cube3>().getLed().setState(false);
                activatedObject = null;
            }
        }
	}
}
