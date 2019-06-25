using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples {
    ///<summary>
    ///Digital representation of a physical arduino button.
    ///</summary>
    public class Button : DeviceComponent {

        ///<summary>
        ///State of the button, whether it's pressed down or not.
        ///</summary>
        private bool state;

        ///<summary>
        ///Bool on whether the button has just been pressed or not.
        ///</summary>
        private bool pressed;
        ///<summary>
        ///Bool on whether the button has just been released or not.
        ///</summary>
        private bool released;

        //Unity update method
        void Update()
        {
            pressed = false;
            released = false;
        }

        //Unity start method used for initialization
        public override void Start(){
            base.Start(); //DeviceComponent's start.
            state = false;
            pressed = false;
            released = false;
        }

        ///<summary>
        ///Method called when the device receives a button press event over MQTT.
        ///Sets the pressed and state variables to true.
        ///</summary>
        void OnButtonPress()
        {
            pressed = true;
            state = true;
            device.InvokeEvent("button.press");
        }

        ///<summary>
        ///Method called when the device receives a button release event over MQTT.
        ///Sets the released variable to true and state variable to false.
        ///</summary>
        void OnButtonRelease()
        {
            released = true;
            state = false;
            device.InvokeEvent("button.release");
        }

        ///<summary>
        ///Returns the released variable that is set to true when the device receives a button release event over MQTT
        ///</summary>
        ///<returns>Just released boolean.</returns>
        public bool JustReleased()
        {
            return released;
        }

        ///<summary>
        ///Returns the pressed variable that is set to true when the device receives a button press event over MQTT.
        ///</summary>
        ///<returns>Just pressed boolean.</returns>
        public bool JustPressed()
        {
            return pressed;
        }

        ///<summary>
        ///Returns the state the button is currently in. True is pressed, false is not.
        ///</summary>
        ///<returns>Button state.</returns>
        public bool IsPressed()
        {
            return state;
        }

        public override void UpdateComponent(string eventType, byte[] payload)
        {
            if(eventType == "state"){
                if(payload[0] == 1){
                    OnButtonPress();
                }else{
                    OnButtonRelease();
                }
            }
        }
    }
}