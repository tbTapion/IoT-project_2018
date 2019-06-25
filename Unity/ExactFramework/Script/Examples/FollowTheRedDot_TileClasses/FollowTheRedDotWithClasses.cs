using System.Collections;
using System.Collections.Generic;
using ExactFramework.Configuration.Examples;
using ExactFramework.Handlers;
using UnityEngine;

///<summary>
///Follow the red dot/follow the light game example with most of the game logic in the objects.
///Mostly used for setting up the game and start it.
///</summary>
public class FollowTheRedDotWithClasses : MonoBehaviour
{
    ///<summary>
    ///MQTT handler object reference.
    ///</summary>
    private MQTTHandler mqttHandler;

    ///<summary>
    ///List of all the tiles/game's objects.
    ///</summary>
    List<IGameTile> tileList = new List<IGameTile>();

    ///<summary>
    ///Boolean for whether the game setup is done or not.
    ///</summary>
    private bool gameSetupDone;

    // Start is called before the first frame update
    void Start()
    {
        mqttHandler = new MQTTHandler("129.241.104.227");

        for (int i = 0; i < 2; i++)
        {
            GameObject redObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + (i * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Red redTile = redObj.AddComponent<Red>();
            redObj.name = "RedTile" + i;
            tileList.Add(redTile);
            mqttHandler.AddTwinObject(redObj.GetComponent<RedTile>());

            GameObject blueObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + ((2 + i) * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Blue blueTile = blueObj.AddComponent<Blue>();
            blueObj.name = "BlueTile" + i;
            tileList.Add(blueTile);
            mqttHandler.AddTwinObject(redObj.GetComponent<BlueTile>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        mqttHandler.Update();
        if (mqttHandler.AllDevicesConnected())
        {
            if (gameSetupDone == false)
            {
                SetupAndPickTile();
            }
        }
    }

    ///<summary>
    ///Called when all the devices have connected. Gives all the tile objects the list of all tiles so they can extract the other tiles for their otherTileList lists.
    ///Picks one random tile and calls its SetActive method to start the game.
    ///</summary>
    private void SetupAndPickTile()
    {
        //Giving all tiles a tile list.
        foreach (IGameTile gameTile in tileList)
        {
            gameTile.SetOtherTileList(tileList);
        }
        //Picking a random tile to start with
        IGameTile startTile = tileList[Random.Range(0, tileList.Count)];
        startTile.SetActive();
        //Setting game setup bool to true so we don't enter this function again. 
        gameSetupDone = true;
    }
}
