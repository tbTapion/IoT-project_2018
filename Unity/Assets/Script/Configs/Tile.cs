using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : TwinObject
{
    protected RingLight ringLight;
    protected TimeOfFlight timeOfFlight;
    protected TonePlayer tonePlayer;

    protected IMU imu;

    // Start is called before the first frame update
    private void Start()
    {
        configName = "tile";
        ringLight = gameObject.AddComponent<RingLight>();
        ringLight.Init(24);
        timeOfFlight = gameObject.AddComponent<TimeOfFlight>();
        tonePlayer = gameObject.AddComponent<TonePlayer>();
        imu = gameObject.AddComponent<IMU>();
    }

    protected override void UpdateComponent(EventMessage e)
    {
        if (e.component == "timeofflight")
        {
            timeOfFlight.SetDistance(e.value);
        }
        else if (e.component == "ringlight")
        {
            if (e.name == "state")
            {
                ringLight.SetState(e.state);
            }
            else if (e.name == "color")
            {   
                Color tempColor = new Color(e.payload[0]/256.0f,e.payload[1]/256.0f,e.payload[2]/256.0f);
                ringLight.SetColor(tempColor);
            }
            else if (e.name == "numOfLeds")
            {
                ringLight.SetNumOfLeds(e.value);
            }
        }
        else if (e.component == "imu")
        {
            if(e.name == "tapped"){
                SendMessage("OnTapped");
            }else if(e.name == "rotation"){
                int roll = e.payload[0]+(e.payload[1]*256);
                int pitch = e.payload[2]+(e.payload[3]*256);
                int yaw = e.payload[4]+(e.payload[5]*256);
                imu.SetRotation(roll,pitch,yaw);
            }
        }
    }
}
