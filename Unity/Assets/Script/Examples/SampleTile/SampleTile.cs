using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTile : BlueTile
{

    private MQTTHandler mqttHandler;

    Color[] colors = new Color[] { Color.red, Color.green, Color.blue };

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        mqttHandler = new MQTTHandler("129.241.104.227"); // May need to change. 
        mqttHandler.addTwinObject(this);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        mqttHandler.update();
        if (getLinkStatus())
        {

            if (imu.justTapped())
            {
                ringLight.setColor(Color.green);
                ringLight.setNumOfLeds(ringLight.getMaxNumLeds());
                ringLight.setState(true);
                //tonePlayer.playTone(300,100);
            }
            else if (timeOfFlight.getMeasuring())
            {
                ringLight.setColor(Color.blue);
                int calcLeds = (int)((timeOfFlight.getDistance() / 200f) * ringLight.getMaxNumLeds());
                ringLight.setNumOfLeds(calcLeds);
                if (ringLight.getState() == false)
                {
                    ringLight.setState(true);
                }
            }
            else
            {
                if (ringLight.getState())
                {
                    ringLight.setState(false);
                }
            }
        }
    }
}
