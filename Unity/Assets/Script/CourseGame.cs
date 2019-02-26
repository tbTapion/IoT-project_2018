using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseGame : MonoBehaviour
{
    private MQTTHandler mQTTHandler;
    int numberOfObjects = 3;
    public List<CustomTile> tileList;
    bool playMode;

    public List<CustomTile> courseOrder;
    int currentTile = 0;
    // Start is called before the first frame update
    void Start()
    {
        //mqttHandler = new MQTTHandler();
        tileList = new List<CustomTile>();
        courseOrder = new List<CustomTile>();

        //Test object
        for(int i = 0; i<numberOfObjects; i++){
            GameObject obj = Instantiate(Resources.Load("Prefabs/Tile"),new Vector3(-1.6f + (i*1.05f), 0.0f, 0.0f),Quaternion.identity) as GameObject;
            CustomTile tile = obj.AddComponent<CustomTile>();
            tileList.Add(tile);
            //tile.setupWaiting();
            //mqttHandler.addTwinObject(obj.AddComponent<CustomTile>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playMode){
            if(courseOrder[currentTile].active == false){
                currentTile = (currentTile + 1) % courseOrder.Count;
                courseOrder[currentTile].setActive(true);
            }
        }else{
            //wait for activation
            if(courseOrder.Count == tileList.Count){
                courseOrder[currentTile].setActive(true);
            }else{
                foreach(CustomTile tile in tileList){
                    if(tile.getIMU().justTapped()){
                        courseOrder.Add(tile);
                        //tile.readyState();
                    }
                }
            }
        }
    }
}
