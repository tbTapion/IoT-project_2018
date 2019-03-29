using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTile : TwinObject
{

    private bool debug = true;

    protected RingLight ringLight;
    protected TimeOfFlight timeOfFlight;
    protected TonePlayer tonePlayer;
    protected IMU imu;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        configName = "bluetile";
        ringLight = gameObject.AddComponent<RingLight>();
        ringLight.Init(24);
        timeOfFlight = gameObject.AddComponent<TimeOfFlight>();
        tonePlayer = gameObject.AddComponent<TonePlayer>();
        imu = gameObject.AddComponent<IMU>();
        if (transform != null)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Start();
        if (debug)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        SendMessage("OnTapped");
                    }
                }
            }
        }
    }

    protected override void UpdateComponent(EventMessage e)
    {
        if (e.component == "timeofflight")
        {
            timeOfFlight.SetDistance(e.value);
            timeOfFlight.SetMeasuringDistance(e.state);
        }
        else if (e.component == "ringlight")
        {
            if (e.name == "state")
            {
                ringLight.SetState(e.state);
            }
            else if (e.name == "color")
            {
                Color tempColor = new Color(e.payload[0] / 256.0f, e.payload[1] / 256.0f, e.payload[2] / 256.0f);
                ringLight.SetColor(tempColor);
            }
            else if (e.name == "numOfLeds")
            {
                ringLight.SetNumOfLeds(e.value);
            }
        }
        else if (e.component == "imu")
        {
            if (e.name == "tapped")
            {
                SendMessage("OnTapped");
            }
        }
    }
}
