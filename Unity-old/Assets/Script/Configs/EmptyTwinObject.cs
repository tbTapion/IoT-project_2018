using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Configuration
{   
    ///<summary>
    /// Empty shell of a TwinObject subclass. 
    ///</summary>
    public class EmptyTwinObject : TwinObject
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            configName = "emptytwinobject";
            //Code here
            //Example AddDeviceComponent<Buttton>();
            //Example AddDeviceComponent<Led>("led1");
            //Example AddDeviceComponent<Led>("led2");
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            //Code here
        }
    }
}
