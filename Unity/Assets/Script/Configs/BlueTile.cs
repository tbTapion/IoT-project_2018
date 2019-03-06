using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTile : TwinObject
{
    
    protected RingLight ringLight;
    protected TimeOfFlight timeOfFlight;
    protected TonePlayer tonePlayer;
    protected IMU imu;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        configName = "bluetile";
        ringLight = new RingLight(this, transform.Find("RingLight").GetComponentsInChildren<RingLightLed>());        
        timeOfFlight = new TimeOfFlight(this);
        tonePlayer = new TonePlayer(this);
        imu = new IMU(this);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        ringLight.update();
        imu.update();
    }   

    protected override void updateComponent(EventMessage e){
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
