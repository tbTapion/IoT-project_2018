﻿using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using UnityEngine;

namespace ExactFramework.Configuration.Examples{
    ///<summary>
    ///Digital representation of a device wuth a ring light, tone player, and an inertial measurement unit.
    ///</summary>
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
            ringLight = AddDeviceComponent<RingLight>("ringlight");
            ringLight.Init(12);
            tonePlayer = AddDeviceComponent<TonePlayer>("toneplayer");
            imu = AddDeviceComponent<IMU>("imu");
            if (transform != null)
            {
                GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }

            AddEventListener("imu.tapped", OnTapped);
            AddEventListener("imu.rotationchanged", OnRotationChange);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
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

        void OnTapped(){

        }

        void OnRotationChange(){
            
        }
    }
}
