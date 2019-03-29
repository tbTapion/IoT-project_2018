using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BlueTile))]
public class SampleTile : MonoBehaviour
{

    private MQTTHandler mqttHandler;

    Color[] colors = new Color[] { Color.red, Color.green, Color.blue };

    // Start is called before the first frame update
    void Start()
    {
        mqttHandler = new MQTTHandler("129.241.104.227"); // May need to change. 
        mqttHandler.AddTwinObject(GetComponent<TwinObject>());
    }

    // Update is called once per frame
    void Update()
    {
        mqttHandler.Update();
        if (mqttHandler.AllDevicesConnected())
        {
            TimeOfFlight timeOfFlight = GetComponent<TimeOfFlight>();
            RingLight ringLight = GetComponent<RingLight>();
            if (timeOfFlight.GetMeasuring())
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

    void OnTapped()
    {
        RingLight ringLight = GetComponent<RingLight>();
        ringLight.SetColor(Color.green);
        ringLight.SetNumOfLeds(ringLight.GetMaxNumLeds());
        ringLight.SetState(true);
        GetComponent<TonePlayer>().PlayTone(300,100);
    }
}
