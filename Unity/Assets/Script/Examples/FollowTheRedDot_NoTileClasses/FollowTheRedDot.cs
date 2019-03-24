using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheRedDot : MonoBehaviour
{

    private MQTTHandler mqttHandler;

    public List<TwinObject> tileList = new List<TwinObject>(); //List of all tiles
    public TwinObject activatedObject; //Activated object variable

    private int state = 0; //Game state variable

    // Use this for initialization
    void Start()
    {
        //MQTT handler. Takes care of the connection to the RPI and sending/receiving messages.
        mqttHandler = new MQTTHandler(""); //Enter IP here

        //Test object
        for (int i = 0; i < 2; i++)
        {
            //Creates a tile game object based on the tile prefab, adds the RedTile component script and adds it to the tile list and mqtt handler
            GameObject redObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + (i * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
            RedTile redTile = redObj.AddComponent<RedTile>();
            redTile.setName("RedTile" + i);
            tileList.Add(redTile);
            mqttHandler.addTwinObject(redTile);
        }

        for (int i = 2; i < 4; i++)
        {
            //Creates a tile game object based on the tile prefab, adds the BlueTile component script and adds it to the tile list and mqtt handler
            GameObject blueObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + (i * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
            BlueTile blueTile = blueObj.AddComponent<BlueTile>();
            blueTile.setName("BlueTile" + i);
            tileList.Add(blueTile);
            mqttHandler.addTwinObject(blueTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mqttHandler.update(); //MQTT handler's update function. Handles updating all the objects based on incoming messages.
        if (mqttHandler.allDevicesConnected()) //Checks to see if all devices are connected.
        {
            switch (state)
            {
                case 1:
                    redDotPlay(); //play mode state function
                    break;
                case 0:
                    waitAndPickTile(); //setup mode state function
                    break;
            }
        }
    }

    private void redDotPlay()
    {
        bool activatedTapped = false; //Tapped variable activating on a tap event from the activated tile.
        if (activatedObject.GetType() == typeof(RedTile)) // Checks to see if the activated tile is of type RedTile
        {
            RedTile redTile = activatedObject as RedTile; //Sets a redtile variable to handle the activated red tile
            if (redTile.getIMU().justTapped()) // Gets the tapped event from the IMU
            {
                redTile.getRingLight().setState(false); //Turns off the light of the activated tile
                activatedTapped = true; // Sets the activated is tapped bool to true
            }
        }
        else if (activatedObject.GetType() == typeof(BlueTile)) // Same as red tile above, but for blue tile
        {
            BlueTile blueTile = activatedObject as BlueTile;
            if (blueTile.getIMU().justTapped())
            {
                blueTile.getRingLight().setState(false);
                activatedTapped = true;
            }
        }
        if (activatedTapped) //If the activated object has been tapped
        {
            List<TwinObject> temp = new List<TwinObject>(tileList); //Creates a copy of the tile list
            temp.Remove(activatedObject); //Removes the former activated tile.
            activatedObject = temp[Random.Range(0, temp.Count)]; //Selects a new activated tile
            if (activatedObject.GetType() == typeof(RedTile)) // Type check of the activated tile
            {
                RedTile redTile = activatedObject as RedTile; // Type variable for the activated tile
                redTile.getRingLight().setState(true); //Sets the light of the activated tile.
            }
            else if (activatedObject.GetType() == typeof(BlueTile)) //Same as for red tile above
            {
                BlueTile blueTile = activatedObject as BlueTile;
                blueTile.getRingLight().setState(true);
            }
        }
    }

    private void waitAndPickTile()
    {
        activatedObject = tileList[Random.Range(0, tileList.Count)]; //Picks a random tile from the tile list.
        if (activatedObject.GetType() == typeof(RedTile)) //Type check of the picked tile from the tile list
        {
            RedTile redTile = activatedObject as RedTile; //Type variable for the tile
            redTile.getRingLight().setState(true); //Sets the light of the activated tile
        }
        else if (activatedObject.GetType() == typeof(BlueTile)) //Same as for red tile above
        {
            BlueTile blueTile = activatedObject as BlueTile;
            blueTile.getRingLight().setState(true);
        }
        state = 1; //Sets the game state to play mode
    }
}
