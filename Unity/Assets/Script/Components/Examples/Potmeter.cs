using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples
{
    ///<summary>
    ///Digital representation of a potmeter component for Arduinos.
    ///</summary>
    public class Potmeter : DeviceComponent
    {

        ///<summary>
        ///Current value of the potmeter. Updates for the first time when the value changes on the physical component.
        ///</summary>
        private int value = 0;

        ///<summary>
        ///Sets the value of the potmeter. Called when the device receives a potmeter update over MQTT.
        ///</summary>
        ///<param name="value">Potmeter value to set.</param>
        private void SetValue(int value)
        {
            this.value = value;
        }

        ///<summary>
        ///Returns the current value of the potmeter. Returns the default 0 if no update has happened.
        ///</summary>
        ///<returns>Current potmeter value.</returns>
        public int GetValue()
        {
            return value;
        }

        public override void UpdateComponent(string eventType, byte[] payload)
        {
            if(eventType == "value"){
                int parsedValue = 0;
                for(int i = 0; i<payload.Length; i++){
                    parsedValue += (int)(payload[i] * Mathf.Pow(256,i));
                }
                SetValue(parsedValue);
            }
        }
    }
}
