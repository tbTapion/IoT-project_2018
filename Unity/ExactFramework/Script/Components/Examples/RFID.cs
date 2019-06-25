using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples
{
    public class RFID : DeviceComponent
    {

        byte[] lastReadID;

        public byte[] GetLastReadID(){
            return lastReadID;
        }

        public override void UpdateComponent(string eventType, byte[] payload)
        {
            if (eventType == "read") //Subject to change
            {
                lastReadID = payload;
                device.InvokeEvent("rfid.read");
            }
        }
    }
}
