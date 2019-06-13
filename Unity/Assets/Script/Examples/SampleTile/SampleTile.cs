using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using ExactFramework.Configuration;
using ExactFramework.Configuration.Examples;
using ExactFramework.Handlers;
using UnityEngine;

[RequireComponent(typeof(BlueTile))]
///<summary>
///An example of a use of the framework to create a sample tile that reacts to taps on an IMU, and measures distance with a time of flight distance sensor.
///Lights up green on taps and blue when measuring distances. Uses the number of leds on its ringlight to show the distance.
///</summary>
public class SampleTile : MonoBehaviour
{
    ///<summary>
    ///MQTT handler class object field.
    ///</summary>
    private MQTTHandler mqttHandler;

    ///<summary>
    ///BlueTile object class reference. Gotten with GetComponent from the gameobject.
    ///</summary>
    BlueTile blueTile;

    ///<summary>
    ///List of a few colors to make color picking easier. Not used.
    ///</summary>
    Color[] colors = new Color[] { Color.red, Color.green, Color.blue };

    // Start is called before the first frame update
    void Start()
    {
        mqttHandler = new MQTTHandler("129.241.104.227"); // May need to change. 
        mqttHandler.AddTwinObject(GetComponent<TwinObject>());

        blueTile = GetComponent<BlueTile>();
        blueTile.AddEventListener("OnTapped", OnTapped);
    }

    // Update is called once per frame
    void Update()
    {
        mqttHandler.Update();
        if (mqttHandler.AllDevicesConnected())
        {
            TimeOfFlight timeOfFlight = blueTile.GetDeviceComponent<TimeOfFlight>();
            RingLight ringLight = blueTile.GetDeviceComponent<RingLight>();
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

    ///<summary>
    ///Old function when Unity's SendMessage functionality was used. Will be used when a proper event handler is in place.
    ///</summary>
    void OnTapped()
    {
        RingLight ringLight = blueTile.GetDeviceComponent<RingLight>();
        ringLight.SetColor(Color.green);
        ringLight.SetNumOfLeds(ringLight.GetMaxNumLeds());
        ringLight.SetState(true);
        blueTile.GetDeviceComponent<TonePlayer>().PlayTone(300,100);
    }
}
