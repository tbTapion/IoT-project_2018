using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : BlueTile
{

    List<TwinObject> otherTiles;

    bool active;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if(imu.justTapped() && active){
            setActive(false);
            TwinObject nextObject = otherTiles[Random.Range(0, otherTiles.Count)];
            if(nextObject.GetType() == typeof(Red)){
                (nextObject as Red).setActive(true);
            }else if(nextObject.GetType() == typeof(Blue)){
                (nextObject as Blue).setActive(true);
            }
        }
        base.Update(); //Update changes the IMU value and needs to be called last.
    }

    public void setActive(bool active){
        this.active = active;
        ringLight.setState(active);
        if(active){
            tonePlayer.playTone(300,200);
        }
    }

    public void setOtherTileList(List<TwinObject> otherTiles){
        List<TwinObject> temp = new List<TwinObject>(otherTiles);
        temp.Remove(this);
        this.otherTiles = temp;
    }
}
