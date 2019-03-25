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
    public override void Start()
    {
        base.Start();
        configName = "tile";
        ringLight = new RingLight(this, 12, transform);
        timeOfFlight = new TimeOfFlight(this);
        tonePlayer = new TonePlayer(this);
        imu = new IMU(this);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        ringLight.update();
        imu.update();
    }

    protected override void updateComponent(EventMessage e)
    {
        if (e.component == "timeofflight")
        {
            timeOfFlight.setDistance(e.value);
        }
        else if (e.component == "ringlight")
        {
            if (e.name == "state")
            {
                ringLight.setState(e.state);
            }
            else if (e.name == "color")
            {   
                Color tempColor = new Color(e.payload[0]/256.0f,e.payload[1]/256.0f,e.payload[2]/256.0f);
                ringLight.setColor(tempColor);
            }
            else if (e.name == "numOfLeds")
            {
                ringLight.setNumOfLeds(e.value);
            }
        }
        else if (e.component == "imu")
        {
            if(e.name == "tapped"){
                imu.setTapped();
            }else if(e.name == "rotation"){
                int roll = e.payload[0]+(e.payload[1]*256);
                int pitch = e.payload[2]+(e.payload[3]*256);
                int yaw = e.payload[4]+(e.payload[5]*256);
                imu.setRotation(roll,pitch,yaw);
            }
        }
    }

    public RingLight getRingLight(){
        return ringLight;
    }

    public TimeOfFlight getTimeOfFlight(){
        return timeOfFlight;
    }

    public TonePlayer getTonePlayer(){
        return tonePlayer;
    }

    public IMU getIMU(){
        return imu;
    }
}
