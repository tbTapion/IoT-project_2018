using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAndRedExample : MonoBehaviour
{
    MQTTHandler mqttHandler;
    // Start is called before the first frame update
    void Start()
    {
        /* mqttHandler = new MQTTHandler();
        GameObject obj = Instantiate(Resources.Load("Prefabs/Tile"),new Vector3(-1.6f + (i*1.05f), 0.0f, 0.0f),Quaternion.identity) as GameObject;
        CustomTile tile = obj.AddComponent<CustomTile>();
        tileList.Add(tile);
        tile.setGameLogic(this);
        mqttHandler.addTwinObject(obj.AddComponent<CustomTile>());*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
