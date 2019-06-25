using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using ExactFramework.Configuration;
using ExactFramework.Configuration.Examples;
using ExactFramework.Handlers;
using UnityEngine;

///<summary>
///A follow the red dot/follow the light game example.
public class FollowTheRedDot : MonoBehaviour
{

    ///<summary>
    ///MQTT Handler object reference.
    ///</summary>
    private MQTTHandler mqttHandler;

    ///<summary>
    ///List of all the game objects used in the game.
    ///</summary>
    public List<TwinObject> tileList = new List<TwinObject>(); //List of all tiles

    ///<summary>
    ///The currently activated game object.
    ///</summary>
    public TwinObject activatedObject; //Activated object variable

    ///<summary>
    ///The state the game is currently in. 0 is setting up, 1 is playing.
    ///</summary>
    private int state = 0; //Game state variable

    // Use this for initialization
    void Start()
    {
        //MQTT handler. Takes care of the connection to the RPI and sending/receiving messages.
        mqttHandler = new MQTTHandler("129.241.104.227"); //Enter IP here

        //Test object
        for (int i = 0; i < 2; i++)
        {
            //Creates a tile game object based on the tile prefab, adds the RedTile component script and adds it to the tile list and mqtt handler
            GameObject redObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + (i * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
            RedTile redTile = redObj.AddComponent<RedTile>();
            redTile.SetName("RedTile" + i);
            tileList.Add(redTile);
            mqttHandler.AddTwinObject(redTile);
        }

        for (int i = 2; i < 4; i++)
        {
            //Creates a tile game object based on the tile prefab, adds the BlueTile component script and adds it to the tile list and mqtt handler
            GameObject blueObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + (i * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
            BlueTile blueTile = blueObj.AddComponent<BlueTile>();
            blueTile.SetName("BlueTile" + i);
            tileList.Add(blueTile);
            mqttHandler.AddTwinObject(blueTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mqttHandler.Update(); //MQTT handler's update function. Handles updating all the objects based on incoming messages.
        if (mqttHandler.AllDevicesConnected()) //Checks to see if all devices are connected.
        {
            switch (state)
            {
                case 1:
                    RedDotPlay(); //play mode state function
                    break;
                case 0:
                    WaitAndPickTile(); //setup mode state function
                    break;
            }
        }
    }

    ///<summary>
    ///Play logic method called every update after the game is finished setting up.
    ///Checks for a tap event and picks a new game object.
    ///</summary>
    private void RedDotPlay()
    {
        IMU imu = activatedObject.GetComponent<IMU>();
        if (imu)
        {
            imu.JustTapped();
            RingLight ringLight = activatedObject.GetComponent<RingLight>();
            if (ringLight)
            {
                ringLight.SetState(false);
            }
            List<TwinObject> temp = new List<TwinObject>(tileList);
            temp.Remove(activatedObject);
            TwinObject nextObject = temp[Random.Range(0, temp.Count)];
            ringLight = activatedObject.GetComponent<RingLight>();
            if (ringLight)
            {
                ringLight.SetState(true);
            }
            activatedObject = nextObject;
        }
    }

    ///<summary>
    ///Setup method called every update before finishing the setup. Called when all devices have connected to the system.
    ///Picks one random device and sets it as active.
    ///</summary>
    private void WaitAndPickTile()
    {
        activatedObject = tileList[Random.Range(0, tileList.Count)]; //Picks a random tile from the tile list.
        RingLight ringLight = activatedObject.GetComponent<RingLight>();
        if (ringLight)
        {
            ringLight.SetState(true);
            state = 1; //Sets the game state to play mode
        }
    }
}
