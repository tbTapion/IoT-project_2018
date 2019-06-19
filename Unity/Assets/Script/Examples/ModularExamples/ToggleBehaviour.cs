using ExactFramework.Component.Examples;
using ExactFramework.Configuration.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MyTile))]
public class ToggleBehaviour : MonoBehaviour
{
    MyTile tile;
    RingLight ringLight;
    TonePlayer tonePlayer;

    // Start is called before the first frame update
    void Start()
    {
        tile = GetComponent<MyTile>();
        ringLight = tile.GetDeviceComponent<RingLight>();
        tonePlayer = tile.GetDeviceComponent<TonePlayer>();

        tile.AddEventListener("OnTapped", OnTapped);
    }

    void OnTapped()
    {
        ringLight.Toggle();
        tonePlayer.PlayTone(300, 20);
    }
}
