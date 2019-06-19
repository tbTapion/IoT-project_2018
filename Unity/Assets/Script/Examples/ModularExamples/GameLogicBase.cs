using ExactFramework.Configuration;
using ExactFramework.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicBase : MonoBehaviour
{

    private MQTTHandler mqttHandler;

    public string MQTTServerAddress = ""; //Change this if you haven't in the inspector
    public bool waitForAllConnected;
    public bool allDevicesConnected;

    public List<TwinObject> devicesInScene = new List<TwinObject>();

    // Start is called before the first frame update
    void Start()
    {
        mqttHandler = new MQTTHandler(MQTTServerAddress);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForAllConnected)
        {
            if (mqttHandler.AllDevicesConnected())
            {
                allDevicesConnected = true;
            }
        }
    }

    public List<T> GetDevicesWithBehavior<T>()
    {
        List<T> listOfObjects = new List<T>();
        foreach(TwinObject to in devicesInScene)
        {
            listOfObjects.Add(to.GetComponent<T>());
        }
        return listOfObjects;
    }
}
