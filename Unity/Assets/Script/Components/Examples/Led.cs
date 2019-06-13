using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples
{
    ///<summary>
    ///Digiral representation of a simple LED.
    ///</summary>
    public class Led : DeviceComponent
    {
        ///<summary>
        ///Current state of the LED. Defaults to false.
        ///</summary>
        protected bool state = false;
        ///<summary>
        ///Whether the LED should blink or not.
        ///</summary>
        protected bool heartbeat;
        ///<summary>
        ///The time before change of state when the LED blinks.
        ///</summary>
        protected int heartbeatTime;

        ///<summary>
        ///Toggles the current state of the LED between on and off. Calls the SetState method.
        ///</summary>
        public void Toggle()
        {
            SetState(!state);
        }

        ///<summary>
        ///Sets the state of the LED from the bool parameter. Also sends a state change message over MQTT.
        ///</summary>
        ///<param name="state">Boolean value to set the state to.</param>
        public void SetState(bool state)
        {
            this.state = state;
            device.SendActionMessage(this.id, (state ? 1 : 0).ToString());
        }

        ///<summary>
        ///Returns the current state of the LED.
        ///</summary>
        ///<returns>Boolean value of the state.</returns>
        public bool GetState()
        {
            return state;
        }

        ///<summary>
        ///Sets the time delay for the blinking of the LED.
        ///</summary>
        ///<param name="heartbeatTime">Time delay for the blinking.</param>
        public void SetHeartbeatTime(int heartbeatTime)
        {
            int tempHeartbeatTime = Mathf.Min(heartbeatTime, 70);
            tempHeartbeatTime = Mathf.RoundToInt((tempHeartbeatTime / 70.0f) * 4);
            if (tempHeartbeatTime != this.heartbeatTime)
            {
                this.heartbeatTime = tempHeartbeatTime;
                device.SendActionMessage("led/heartbeat", this.heartbeatTime.ToString());
                if (this.heartbeatTime > 0)
                {
                    this.heartbeat = true;
                }
                else
                {
                    this.heartbeat = false;
                }
            }
        }
        
        ///<summary>
        ///Gets the time delay set for the blinking of the LED
        ///</summary>
        ///<returns>Time delay for the blinking.</returns>
        public int GetHeartbeatTime()
        {
            return heartbeatTime;
        }

        ///<summary>
        ///Gets whether the blinking's state is set on or off.
        ///</summary>
        ///<returns>Bool for blinking.</returns>
        public bool GetHeartbeatState()
        {
            return heartbeat;
        }
    }
}