using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : BlueTile, IGameTile
{

    List<IGameTile> otherTiles;

    bool active;

    // Start is called before the first frame update
    private void Start()
    {
        active = false;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    void OnTapped()
    {
        if (active)
        {
            active = false;
            ringLight.SetState(false);
            IGameTile nextObject = otherTiles[Random.Range(0, otherTiles.Count)];
            nextObject.SetActive();
        }
    }

    public void SetActive()
    {
        ringLight.SetState(true);
        //tonePlayer.PlayTone(300,200);
    }

    public void SetOtherTileList(List<IGameTile> tileList)
    {
        otherTiles = new List<IGameTile>(tileList);
        otherTiles.Remove(this);
    }
}
