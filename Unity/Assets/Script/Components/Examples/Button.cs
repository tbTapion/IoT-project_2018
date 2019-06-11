﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples {
    public class Button : DeviceComponent {

        private bool state;
        private bool pressed;
        private bool released;

        void Update()
        {
            pressed = false;
            released = false;
        }

        public override void Start(){
            base.Start();
            state = false;
            pressed = false;
            released = false;
        }

        void OnButtonPress()
        {
            pressed = true;
            state = true;
        }

        void OnButtonRelease()
        {
            released = true;
            state = false;
        }

        public bool JustReleased()
        {
            return released;
        }

        public bool JustPressed()
        {
            return pressed;
        }

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