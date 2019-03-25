using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTile : TwinObject
{

    private bool debug = true;

    protected RingLight ringLight;
    public TimeOfFlight timeOfFlight;
    protected TonePlayer tonePlayer;
    protected IMU imu;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        configName = "bluetile";
        ringLight = new RingLight(this, 24, transform);
        timeOfFlight = new TimeOfFlight(this);
        tonePlayer = new TonePlayer(this);
        imu = new IMU(this);
        if (transform != null)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        ringLight.update();
        imu.update();

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
                        Debug.Log("A Blue Tile tapped");
                        imu.setTapped();
                    }
                }
            }
        }
    }

    protected override void updateComponent(EventMessage e)
    {
        if (e.component == "timeofflight")
        {
            timeOfFlight.setDistance(e.value);
            timeOfFlight.setMeasuring(e.state);
        }
        else if (e.component == "ringlight")
        {
            if (e.name == "state")
            {
                ringLight.setState(e.state);
            }
            else if (e.name == "color")
            {
                Color tempColor = new Color(e.payload[0] / 256.0f, e.payload[1] / 256.0f, e.payload[2] / 256.0f);
                ringLight.setColor(tempColor);
            }
            else if (e.name == "numOfLeds")
            {
                ringLight.setNumOfLeds(e.value);
            }
        }
        else if (e.component == "imu")
        {
            if (e.name == "tapped")
            {
                imu.setTapped();
            }
        }
    }

    public RingLight getRingLight()
    {
        return ringLight;
    }

    public TimeOfFlight getTimeOfFlight()
    {
        return timeOfFlight;
    }

    public TonePlayer getTonePlayer()
    {
        return tonePlayer;
    }

    public IMU getIMU()
    {
        return imu;
    }
}
