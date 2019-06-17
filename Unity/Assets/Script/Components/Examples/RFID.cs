using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples
{
    public class RFID : DeviceComponent
    {
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        public void ChipDetected(){
            device.InvokeEvent("OnRFIDChipDetected");
        }

        public override void UpdateComponent(string eventType, byte[] payload)
        {
            if (eventType == "detected") //Subject to change
            {

            }
        }
    }
}
