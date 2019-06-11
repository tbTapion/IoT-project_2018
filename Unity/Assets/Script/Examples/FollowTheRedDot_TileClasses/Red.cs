using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using ExactFramework.Configuration.Examples;
using UnityEngine;

[RequireComponent(typeof(RedTile))]
public class Red : MonoBehaviour, IGameTile
{

    List<IGameTile> otherTiles;

    RedTile redTile;

    bool active;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        redTile = GetComponent<RedTile>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTapped()
    {
        if (active) {
            active = false;
            redTile.GetDeviceComponent<RingLight>().SetState(false);
            IGameTile nextObject = otherTiles[Random.Range(0, otherTiles.Count)];
            nextObject.SetActive();
        }
    }

    public void SetActive()
    {
        redTile.GetDeviceComponent<RingLight>().SetState(true);
        redTile.GetDeviceComponent<TonePlayer>().PlayTone(300, 200);
    }

    public void SetOtherTileList(List<IGameTile> tileList)
    {
        otherTiles = new List<IGameTile>(tileList);
        otherTiles.Remove(this);
    }
}
