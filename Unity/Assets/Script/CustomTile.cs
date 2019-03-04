using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTile : Tile
{
    MyGameLogic gameLogic;
    public bool active;

    public List<CustomTile> otherTiles;
    int i = 60;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ringLight.setColor(new Color(0.5f, 0.3f, 0.5f));
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(active){
            //if(imu.justTapped()){
                if(i == 0){
                    otherTiles[Random.Range(0, otherTiles.Count)].setActive(true);;
                    setActive(false);
                    i=60;
                }else{
                    i--;
                }
            //}
        }
    }

    protected override void onEvent(EventMessage e){
        if(e.component == "imu"){
            if(e.name == "tapped"){
                CustomTile temp = otherTiles[Random.Range(0,otherTiles.Count)];
                temp.setActive(true);
                setActive(false);
            }
        }
    }

    public void setActive(bool active){
        this.active = active;
        ringLight.setState(active);
    }
    
    public void setGameLogic(MyGameLogic gameLogic){
        this.gameLogic = gameLogic;
    }

    public void setOtherList(List<CustomTile> tileList){
        otherTiles = new List<CustomTile>(tileList);
        otherTiles.Remove(this);
    }
}
