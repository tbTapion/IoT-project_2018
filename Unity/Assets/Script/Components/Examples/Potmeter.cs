using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples
{
    public class Potmeter : DeviceComponent
    {

        private int value = 0;

        public void SetValue(int value)
        {
            this.value = value;
        }

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
