﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExactFramework.Component;

namespace ExactFramework.Configuration
{
    public class NestedTwinObject : TwinObject
    {   
        //Set if you want to set named per component in their transform. Useful if using multiple of a component like two LEDs and separating them with led1 and led2
        public bool useTransformNames = false;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            for(int i = 0;i<transform.childCount; i++){
                DeviceComponent deviceComponent = transform.GetChild(i).GetComponent<DeviceComponent>();
                if(useTransformNames){
                    AddExistingDeviceComponent(transform.name, deviceComponent);
                }else{
                    AddExistingDeviceComponent(deviceComponent);
                }
            }  
        }

        // Update is called once per frame
        /*protected override void Update()
        {
            base.Update();
        }*/
    }
}
