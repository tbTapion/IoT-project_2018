using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseGame : MonoBehaviour
{
    private const bool Active = true;
    public MQTTHandler mqttHandler;

    public List<TwinObject> tileList;
    
    public enum mode {INIT=0, DEMO=1, PLAY=2};
    public int state = 0;

    public List<TwinObject> courseOrder;
    public int currentTile = 0;
    // Start is called before the first frame update
    void Start()
    {
        mqttHandler = new MQTTHandler("129.241.104.227");
        tileList = new List<TwinObject>();
        courseOrder = new List<TwinObject>();

        GameObject obj = Instantiate(Resources.Load("Prefabs/Tile"), new Vector3(-1.6f + (0 * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
        RedCourseTile redTile = obj.AddComponent<RedCourseTile>();
        tileList.Add(redTile);
        mqttHandler.addTwinObject(redTile);

        GameObject obj2 = Instantiate(Resources.Load("Prefabs/Tile"), new Vector3(-1.6f + (1 * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
        BlueCourseTile blueTile = obj2.AddComponent<BlueCourseTile>();
        tileList.Add(blueTile);
        mqttHandler.addTwinObject(blueTile);
    }

    // Update is called once per frame
    void Update()
    {
        mqttHandler.update();
        if (mqttHandler.allDevicesConnected())
        {
            switch(state){
                case (int)mode.PLAY:
                    playMode();
                    break;
                case (int)mode.DEMO:
                    demoMode();
                    break;
                case (int)mode.INIT:
                    initDevices();
                    break;
            }
        }
    }
    
    public void playMode(){
        if(courseOrder[currentTile].GetType() == typeof(RedCourseTile)){
            RedCourseTile tile = courseOrder[currentTile] as RedCourseTile;
            if(tile.active){
                if(tile.getIMU().justTapped()){
                    tile.setActive(false);
                    currentTile = (currentTile + 1) % courseOrder.Count;
                }
            }else{
                tile.setActive(true);
            }
        }else if(courseOrder[currentTile].GetType() == typeof(BlueCourseTile)){
            BlueCourseTile tile = courseOrder[currentTile] as BlueCourseTile;
            if(tile.active){
                if(tile.getIMU().justTapped()){
                    tile.setActive(false);
                    currentTile = (currentTile + 1) % courseOrder.Count;
                }
            }else{
                tile.setActive(true);
            }
        }
    }

    public void demoMode(){
        foreach(TwinObject to in tileList){
            if(courseOrder.Contains(to) == false){
                if(to.GetType() == typeof(RedCourseTile)){
                    RedCourseTile tile = to as RedCourseTile;
                    if(tile.getIMU().justTapped()){
                        courseOrder.Add(tile);
                        tile.setupReady();
                    }
                }else if(to.GetType() == typeof(BlueCourseTile)){
                    BlueCourseTile tile = to as BlueCourseTile;
                    if(tile.getIMU().justTapped()){
                        courseOrder.Add(tile);
                        tile.setupReady();
                    }
                }
            }
        }
        if(courseOrder.Count == tileList.Count){
            state = (int)mode.PLAY;
             if(courseOrder[currentTile].GetType() == typeof(RedCourseTile)){
                (courseOrder[currentTile] as RedCourseTile).setActive(true);
            }else if(courseOrder[currentTile].GetType() == typeof(BlueCourseTile)){
                (courseOrder[currentTile] as BlueCourseTile).setActive(true);
            }
        }
    }

    public void initDevices(){
        foreach(TwinObject to in tileList){
            if(to.GetType() == typeof(RedCourseTile)){
                RedCourseTile tile = to as RedCourseTile;
                if(!tile.active){
                    tile.setupWaiting();
                }
            }else if(to.GetType() == typeof(BlueCourseTile)){
                BlueCourseTile tile = to as BlueCourseTile;
                if(!tile.active){
                    tile.setupWaiting();
                }
            }
        }
        state = (int)mode.DEMO;
    }

}
