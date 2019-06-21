using ExactFramework.Component.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MyTile))]
public class RingLightPulseBehaviour : MonoBehaviour
{
    MyTile tile;
    RingLight ringLight;

    bool ringLightSwitchedOn = false;

    float lastTime;
    int greenColorValue = 0;

    public int NumberOfLeds = 0;

    // Start is called before the first frame update
    void Start()
    {
        tile = GetComponent<MyTile>();
        ringLight = tile.GetDeviceComponent<RingLight>();
        ringLight.Init(NumberOfLeds);
    }

    // Update is called once per frame
    void Update()
    {
        if (tile.GetLinkStatus())
        {
            if (!ringLightSwitchedOn)
            {
                ringLight.SetState(true);
                ringLightSwitchedOn = true;
            }

            lastTime += Time.deltaTime;

            if(lastTime >= 2)
            {
                Debug.Log("Time reached!");
                greenColorValue = (greenColorValue+1) % 2;
                ringLight.SetColor(new Color(0, greenColorValue, 1));
                lastTime = 0;
            }
        }
    }
}
