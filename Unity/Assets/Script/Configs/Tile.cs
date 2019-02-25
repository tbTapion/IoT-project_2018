using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : TwinObject
{
    protected RingLight ringLight;
    protected TimeOfFlight timeOfFlight;
    protected TonePlayer tonePlayer;

    //protected IMU imu;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        configName = "tile";
        ringLight = new RingLight(this,transform.Find("RingLight").GetComponentsInChildren<RingLightLed>());
        timeOfFlight = new TimeOfFlight(this);
        tonePlayer = new TonePlayer(this);
        //imu = new IMU(this);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        ringLight.update();
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
                string[] temp = e.payload.Split(',');
                Color tempColor = new Color(int.Parse(temp[0]),int.Parse(temp[1]),int.Parse(temp[2]));
                ringLight.setColor(tempColor);
            }
            else if (e.name == "numOfLeds")
            {
                ringLight.setNumOfLeds(e.value);
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
}
