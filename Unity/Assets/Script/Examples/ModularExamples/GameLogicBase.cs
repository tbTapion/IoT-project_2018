using ExactFramework.Configuration;
using ExactFramework.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicBase : MonoBehaviour
{

    private MQTTHandler mqttHandler;

    public string MQTTServerAddress;
    public bool WaitForAllConnected;

    public List<TwinObject> devicesInScene = new List<TwinObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<T> GetDeviceWithBehavior<T>()
    {
        List<T> listOfObjects = new List<T>();
        foreach(TwinObject to in devicesInScene)
        {
            listOfObjects.Add(to.GetComponent<T>());
        }
        return listOfObjects;
    }
}
