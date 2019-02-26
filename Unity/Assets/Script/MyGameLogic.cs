using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameLogic : MonoBehaviour
{
    private MQTTHandler mqttHandler;

    int numberOfObjects = 3;
    public List<CustomTile> tileList = new List<CustomTile>();

    // Start is called before the first frame update
    void Start()
    {
        mqttHandler = new MQTTHandler();

        //Test object
        for(int i = 0; i<numberOfObjects; i++){
            GameObject obj = Instantiate(Resources.Load("Prefabs/Tile"),new Vector3(-1.6f + (i*1.05f), 0.0f, 0.0f),Quaternion.identity) as GameObject;
            CustomTile tile = obj.AddComponent<CustomTile>();
            tileList.Add(tile);
            tile.setGameLogic(this);
            mqttHandler.addTwinObject(obj.AddComponent<CustomTile>());
        }
        foreach(CustomTile tile in tileList){
            tile.setOtherList(tileList);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mqttHandler.update();
        if(mqttHandler.allDevicesConnected()){
            bool anyActive = false;
            foreach(CustomTile tile in tileList){
                if(tile.active){
                    anyActive = tile.active;
                }
            }
            if(anyActive == false){
                CustomTile firstActive = tileList[Random.Range(0, tileList.Count)];
                firstActive.active = true;
            }
        }
    }
}
