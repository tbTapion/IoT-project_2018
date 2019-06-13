using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using UnityEngine;

namespace ExactFramework.Configuration.Examples{
    ///<summary>
    ///Digital representation of a device wuth a ring light, time of flight distance sensor, tone player, and an inertial measurement unit.
    ///</summary>
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
            ringLight = AddDeviceComponent<RingLight>("ringlight");
            ringLight.Init(24);
            timeOfFlight = AddDeviceComponent<TimeOfFlight>("timeofflight");
            tonePlayer = AddDeviceComponent<TonePlayer>("toneplayer");
            imu = AddDeviceComponent<IMU>("imu");
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
                    /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == transform)
                        {
                            SendMessage("OnTapped");
                        }
                    }*/
                }
            }
        }
    }
}
