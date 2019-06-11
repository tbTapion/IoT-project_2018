using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExactFramework.Component;

namespace ExactFramework.Configuration
{
    public class NestedTwinObject : TwinObject
    {   
        public bool useTransformNames = false;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            for(int i = 0;i<transform.childCount; i++){
                DeviceComponent component = transform.GetChild(i).GetComponent<DeviceComponent>();
                if(useTransformNames){
                    AddExistingDeviceComponent(transform.name, component);
                }else{
                    AddExistingDeviceComponent(component);
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
