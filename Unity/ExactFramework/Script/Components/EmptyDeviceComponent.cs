using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component
{
    ///<summary>
    /// Empty shell of a DeviceComponent subclass. 
    ///</summary>
    public class EmptyDeviceComponent : DeviceComponent
    {
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            //Code here
        }

        //Add standard void Update() method if needed.

        public override void UpdateComponent(string eventType, byte[] payload)
        {
            //Only needed if you expect message from the physical device that updates this component.
        }
    }
}