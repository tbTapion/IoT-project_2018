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
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(active){
            if(i == 0){
                otherTiles[Random.Range(0, otherTiles.Count)].setActive(true);;
                setActive(false);
                i = 60;
            }else{
                i--;
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
