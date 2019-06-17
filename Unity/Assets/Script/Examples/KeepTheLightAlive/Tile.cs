using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using ExactFramework.Configuration;
using UnityEngine;

namespace KeepAliveExample
{
    public class Tile : TwinObject
    {
        List<Tile> otherTiles;
        protected RingLight ringLight;
        protected TonePlayer tonePlayer;
        protected IMU imu;

        bool active;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            ringLight = AddDeviceComponent<RingLight>();
            tonePlayer = AddDeviceComponent<TonePlayer>();
            imu = AddDeviceComponent<IMU>();

            AddEventListener("OnTapped", OnTapped);
        }

        void OnTapped(){
            if(active){
                active = false;
                otherTiles[Random.Range(0, otherTiles.Count)].SetActive();
                ringLight.SetState(false);
            }
        }

        public void SetActive()
        {
            active = true;
            ringLight.SetState(true);
            //tonePlayer.PlayTone(200,40);
        }

        public void SetOtherTiles(List<Tile> list)
        {
            otherTiles = new List<Tile>(list);
            otherTiles.Remove(this);
        }
    }
}
