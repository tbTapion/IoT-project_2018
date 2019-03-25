using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheRedDotWithClasses : MonoBehaviour
{
    private MQTTHandler mqttHandler;

    List<TwinObject> tileList = new List<TwinObject>();

    private bool gameSetupDone;

    // Start is called before the first frame update
    void Start()
    {
        mqttHandler = new MQTTHandler("129.241.104.227");

        for (int i = 0; i < 2; i++)
        {
            GameObject redObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + (i * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
            RedTile redTile = redObj.AddComponent<Red>();
            redTile.setName("RedTile" + i);
            tileList.Add(redTile);
            mqttHandler.addTwinObject(redTile);

            GameObject blueObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + ((2 + i) * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
            BlueTile blueTile = blueObj.AddComponent<Blue>();
            blueTile.setName("BlueTile" + i);
            tileList.Add(blueTile);
            mqttHandler.addTwinObject(blueTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mqttHandler.update();
        if (mqttHandler.allDevicesConnected())
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
        foreach (TwinObject to in tileList)
        {
            if (to.GetType() == typeof(Red))
            {
                (to as Red).setOtherTileList(tileList);
            }
            else if (to.GetType() == typeof(Blue))
            {
                (to as Blue).setOtherTileList(tileList);
            }
        }
        //Picking a random tile to start with
        TwinObject startTile = tileList[Random.Range(0, tileList.Count)];
        if (startTile.GetType() == typeof(Red))
        {
            (startTile as Red).setActive(true);
        }
        else if (startTile.GetType() == typeof(Blue))
        {
            (startTile as Blue).setActive(true);
        }
        //Setting game setup bool to true so we don't enter this function again. 
        gameSetupDone = true;
    }
}
