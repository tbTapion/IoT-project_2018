using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using ExactFramework.Configuration.Examples;
using UnityEngine;

[RequireComponent(typeof(BlueTile))]
public class Blue : MonoBehaviour, IGameTile
{

    List<IGameTile> otherTiles;

    BlueTile blueTile;

    bool active;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        blueTile = GetComponent<BlueTile>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTapped()
    {
        if (active)
        {
            active = false;
            blueTile.GetDeviceComponent<RingLight>().SetState(false);
            IGameTile nextObject = otherTiles[Random.Range(0, otherTiles.Count)];
            nextObject.SetActive();
        }
    }

    public void SetActive()
    {
        blueTile.GetDeviceComponent<RingLight>().SetState(true);
        blueTile.GetDeviceComponent<TonePlayer>().PlayTone(300, 200);
    }

    public void SetOtherTileList(List<IGameTile> tileList)
    {
        otherTiles = new List<IGameTile>(tileList);
        otherTiles.Remove(this);
    }
}
