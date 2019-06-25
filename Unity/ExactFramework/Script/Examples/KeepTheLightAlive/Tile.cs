using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using ExactFramework.Configuration;
using UnityEngine;

namespace KeepAliveExample
{
    public class Tile : TwinObject
    {
        KeepTheLightAlive gameLogic;

        List<Tile> otherTiles;
        protected RingLight ringLight;
        protected TonePlayer tonePlayer;
        protected IMU imu;

        bool active;

        private int life;
        private int maxLife;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            ringLight = AddDeviceComponent<RingLight>();
            tonePlayer = AddDeviceComponent<TonePlayer>();
            imu = AddDeviceComponent<IMU>();

            AddEventListener("imu.tapped", OnTapped);
        }

        // Update is called once every frame
        protected override void Update()
        {
            if(active){
                life--;
                float lifeFraction = life/maxLife;
                int currentLedCount = ringLight.GetNumOfLeds();
                currentLedCount = (int)(lifeFraction * ringLight.GetMaxNumLeds());
                ringLight.SetNumOfLeds(currentLedCount);
                if(currentLedCount == 0){
                    gameLogic.SetGameStateGameOver();
                }
            }
        }

        void OnTapped()
        {
            if (active)
            {
                active = false;
                ResetLEDs();
                otherTiles[Random.Range(0, otherTiles.Count)].SetActive();
            }
        }

        public void SetActive()
        {
            active = true;
            life = 60*Random.Range(5, 10);
            maxLife = life;
            ringLight.SetState(true);
            //tonePlayer.PlayTone(200,40);
        }

        public void SetOtherTiles(List<Tile> list)
        {
            otherTiles = new List<Tile>(list);
            otherTiles.Remove(this);
        }

        public void SetGameLogic(KeepTheLightAlive gameLogic){
            this.gameLogic = gameLogic;
        }

        public void ResetLEDs(){
            ringLight.SetNumOfLeds(ringLight.GetMaxNumLeds());
            ringLight.SetState(false);
        }

        public void ToggleLEDs(){
            ringLight.Toggle();
        }
    }
}
