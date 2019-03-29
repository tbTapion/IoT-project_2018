using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTile : TwinObject
{

    private bool debug = true;

    protected RingLight ringLight;
    protected TonePlayer tonePlayer;
    protected IMU imu;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        configName = "redtile";
        ringLight = gameObject.AddComponent<RingLight>();
        ringLight.Init(12);
        tonePlayer = gameObject.AddComponent<TonePlayer>();
        imu = gameObject.AddComponent<IMU>();
        if (transform != null)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
        if (e.component == "ringlight")
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
            if (e.name == "rotation")
            {
                int roll = e.payload[0] + e.payload[1];
                int pitch = e.payload[2] + e.payload[3];
                int yaw = e.payload[4] + e.payload[5];
                imu.SetRotation(roll, pitch, yaw);
            }
            else if (e.name == "tapped")
            {
                SendMessage("OnTapped");
            }
        }
    }
}
