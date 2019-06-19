using ExactFramework.Component.Examples;
using ExactFramework.Configuration.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MyTile))]
public class RedDotBehaviour : MonoBehaviour
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
        ringLight.SetState(false);
    }

    public void SetActive()
    {
        ringLight.SetState(true);
        tonePlayer.PlayTone(400, 30);
    }

    public bool GetActive()
    {
        return ringLight.GetState();
    }

    // Update is called once per frame
    //void Update(){}
}
