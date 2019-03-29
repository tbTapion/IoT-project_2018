using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BlueTile))]
public class Blue : MonoBehaviour, IGameTile
{

    List<IGameTile> otherTiles;

    bool active;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
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
            GetComponent<RingLight>().SetState(false);
            IGameTile nextObject = otherTiles[Random.Range(0, otherTiles.Count)];
            nextObject.SetActive();
        }
    }

    public void SetActive()
    {
        GetComponent<RingLight>().SetState(true);
        //GetComponent<TonePlayer>().PlayTone(300, 200);
    }

    public void SetOtherTileList(List<IGameTile> tileList)
    {
        otherTiles = new List<IGameTile>(tileList);
        otherTiles.Remove(this);
    }
}
