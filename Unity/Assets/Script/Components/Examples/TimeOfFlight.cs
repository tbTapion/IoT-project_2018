using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples
{
    public class TimeOfFlight : DeviceComponent
    {
        private int distance = 9999;
        private bool measuringDistance;

        public void SetDistance(int distance)
        {
            this.distance = distance;
        }

        public int GetDistance()
        {
            return distance;
        }

        public void SetMeasuringDistance(bool measuringDistance)
        {
            this.measuringDistance = measuringDistance;
            if (measuringDistance)
            {
                SendMessage("OnMeasuredDistance");
            }
        }

        public bool GetMeasuring()
        {
            return measuringDistance;
        }

        public override void UpdateComponent(string eventType, byte[] payload)
        {
            if(eventType == "value"){
                int parsedValue = 0;
                for(int i = 0; i<payload.Length; i++){
                    parsedValue += (int)payload[i] * (int)Mathf.Pow(256, i);
                }
                SetDistance(parsedValue);
                SetMeasuringDistance(true);
            }else if(eventType == "off"){
                SetMeasuringDistance(false);
            }
        }
    }
}
