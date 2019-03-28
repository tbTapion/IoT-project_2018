using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheRedDotWithClasses : MonoBehaviour
{
    private MQTTHandler mqttHandler;

    List<IGameTile> tileList = new List<IGameTile>();

    private bool gameSetupDone;

    // Start is called before the first frame update
    void Start()
    {
        mqttHandler = new MQTTHandler("129.241.104.227");

        for (int i = 0; i < 2; i++)
        {
            GameObject redObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + (i * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Red redTile = redObj.AddComponent<Red>();
            redTile.SetName("RedTile" + i);
            tileList.Add(redTile);
            mqttHandler.AddTwinObject(redTile);

            GameObject blueObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + ((2 + i) * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Blue blueTile = blueObj.AddComponent<Blue>();
            blueTile.SetName("BlueTile" + i);
            tileList.Add(blueTile);
            mqttHandler.AddTwinObject(blueTile);
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
                setupAndPickTile();
            }
        }
    }

    private void setupAndPickTile()
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
