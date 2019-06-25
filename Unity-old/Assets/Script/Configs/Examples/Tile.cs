using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using UnityEngine;

namespace ExactFramework.Configuration.Examples{
    ///<summary>
    ///Digital representation of a device wuth a ring light, time of flight distance sensor, tone player, and an inertial measurement unit.
    ///</summary>
    public class Tile : TwinObject
    {
        protected RingLight ringLight;
        protected TimeOfFlight timeOfFlight;
        protected TonePlayer tonePlayer;

        protected IMU imu;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            configName = "tile";
            ringLight = AddDeviceComponent<RingLight>("ringlight");
            ringLight.Init(24);
            timeOfFlight = AddDeviceComponent<TimeOfFlight>("timeofflight");
            tonePlayer = AddDeviceComponent<TonePlayer>("timeofflight");
            imu = AddDeviceComponent<IMU>("imu");

            AddEventListener("imu.tapped", OnTapped);
            AddEventListener("timeofflight.distancevalue", OnDistanceChanged);
        }
        
        void OnTapped(){

        }

        void OnDistanceChanged(){

        }
    }
}
