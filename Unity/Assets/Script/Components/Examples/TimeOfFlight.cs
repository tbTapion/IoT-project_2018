using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples
{
    ///<summary>
    ///Digital representation of a time of flight distance sensor.
    ///</summary>
    public class TimeOfFlight : DeviceComponent
    {
        ///<summary>
        ///Distance measured by the sensor.
        ///</summary>
        private int distance = 9999;
        ///<summary>
        ///Whether the sensor is measuring a distance or not.
        ///</summary>
        private bool measuringDistance;

        ///<summary>
        ///Method called when the a distance is received over MQTT. Sets the distance measured.
        ///</summary>
        ///<param name="distance">Distance value measured.</param>
        private void SetDistance(int distance)
        {
            this.distance = distance;
            device.InvokeEvent("OnDistanceChanged");
        }

        ///<summary>
        ///Gets the distance measured, or last measured, by the sensor.
        ///</summary>
        ///<returns>Distance value.</returns>
        public int GetDistance()
        {
            return distance;
        }

        ///<summary>
        ///Sets whether the sensor is measuring a distance or not. Value received over MQTT.
        ///</summary>
        ///<param name="measuringDistance">Measuring distance boolean.</param>
        private void SetMeasuringDistance(bool measuringDistance)
        {
            this.measuringDistance = measuringDistance;
        }

        ///<summary>
        ///Returns the state of whether the sensor is measuring a distance or not.
        ///</summary>
        ///<returns>Dsitance measuring boolean.</returns>
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
