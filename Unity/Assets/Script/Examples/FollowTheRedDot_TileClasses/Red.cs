using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using ExactFramework.Configuration.Examples;
using UnityEngine;

[RequireComponent(typeof(RedTile))]
///<summary>
///Red game object class. References the RedTile configuration example. Used in a follow the red dot/follow the light game example. 
///Has the IGameTile interface for easier method calls on the same methods between this Red class and a Blue class which also has the same interface.
///</summary>
public class Red : MonoBehaviour, IGameTile
{
    ///<summary>
    ///List of all other IGameTile classes used in the game.
    ///</summary>
    List<IGameTile> otherTiles;

    ///<summary>
    ///Reference to the RedTile class.
    ///</summary>
    RedTile redTile;

    ///<summary>
    ///Game logic boolean for whether this game object is active or not.
    ///</summary>
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

    ///<summary>
    ///Old function when Unity's SendMessage functionality was used. Will be used when a proper event handler is in place.
    ///</summary>
    void OnTapped()
    {
        if (active) {
            active = false;
            redTile.GetDeviceComponent<RingLight>().SetState(false);
            IGameTile nextObject = otherTiles[Random.Range(0, otherTiles.Count)];
            nextObject.SetActive();
        }
    }

    ///<summary>
    ///Method from the IGameTile interface. Used to set the device to an active state. Switches on the lights, and plays a tone.
    ///</summary>
    public void SetActive()
    {
        redTile.GetDeviceComponent<RingLight>().SetState(true);
        redTile.GetDeviceComponent<TonePlayer>().PlayTone(300, 200);
    }
    
    ///<summary>
    ///Method from the IGameTile interface. Used to set the otherTiles list with the other tile devices in the game.
    ///</summary>
    public void SetOtherTileList(List<IGameTile> tileList)
    {
        otherTiles = new List<IGameTile>(tileList);
        otherTiles.Remove(this);
    }
}
