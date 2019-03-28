using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTile : BlueTile
{

    private MQTTHandler mqttHandler;

    Color[] colors = new Color[] { Color.red, Color.green, Color.blue };

    // Start is called before the first frame update
    private void Start()
    {
        mqttHandler = new MQTTHandler("129.241.104.227"); // May need to change. 
        mqttHandler.AddTwinObject(this);
    }

    // Update is called once per frame
    private void Update()
    {
        mqttHandler.Update();
        if (linked)
        {

            if (imu.JustTapped())
            {
                ringLight.SetColor(Color.green);
                ringLight.SetNumOfLeds(ringLight.GetMaxNumLeds());
                ringLight.SetState(true);
                //tonePlayer.playTone(300,100);
            }
            else if (timeOfFlight.GetMeasuring())
            {
                ringLight.SetColor(Color.blue);
                int calcLeds = (int)((timeOfFlight.GetDistance() / 200f) * ringLight.GetMaxNumLeds());
                ringLight.SetNumOfLeds(calcLeds);
                if (ringLight.GetState() == false)
                {
                    ringLight.SetState(true);
                }
            }
            else
            {
                if (ringLight.GetState())
                {
                    ringLight.SetState(false);
                }
            }
        }
    }
}
