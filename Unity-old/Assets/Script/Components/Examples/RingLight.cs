using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples
{
    ///<summary>
    ///Digital representation of a ring light component.
    ///</summary>
    public class RingLight : DeviceComponent
    {
        
        ///<summary>
        ///Change this variable for a uniform color of the ring light.
        ///</summary>
        private Color color;

        ///<summary>
        ///The state of the full ring light, if it's on or off.
        ///</summary>
        private bool state = false;

        ///<summary>
        ///Variable for the maximum number of LEDs for this ring light object. Set once when initialized.
        ///</summary>
        private readonly int maxNumLeds;

        ///<summary>
        ///Variable for number of active LEDs for the ring light. Default 24.
        ///</summary>
        private int numOfLeds = 24;

        ///<summary>
        ///List of individual LEDs in the ring light.
        ///</summary>
        private List<RingLightLed> ledList;

        public override void Start()
        {
            base.Start();
            color = new Color(0, 1f, 0);
            Init(numOfLeds);
        }

        ///<summary>
        ///Initialization method for the ring light. Should be called once, and takes the number of LEDs expected for this ring light.
        ///</summary>
        ///<param name="numberOfLeds">Sets number of LEDs active to start with and the number of LEDs max.</param>
        public void Init(int numberOfLeds)
        {
            ledList = new List<RingLightLed>();
            RingLightLed[] transformLedList = transform.Find("RingLight").GetComponentsInChildren<RingLightLed>();
            if (transformLedList != null)
            {
                this.ledList.AddRange(transformLedList);
            }
            else
            {
                for (int i = 0; i < maxNumLeds; i++)
                {
                    RingLightLed led = gameObject.AddComponent<RingLightLed>();
                    led.SetColor(color);
                    ledList.Add(led);
                }
            }
        }

        ///<summary>
        ///Toggles the state of the ring light. Calls on the SetState method.
        ///</summary>
        public void Toggle()
        {
            SetState(!state);
        }

        ///<summary>
        ///Sets the state of the ring light based on the bool parameter.
        ///</summary>
        ///<param name="state">Boolean to set the state of the ring light.</param>
        public void SetState(bool state)
        {
            this.state = state;
            if (ledList != null)
            {
                UpdateLeds();
            }
            device.SendActionMessage("ringlight/state", state);
        }

        ///<summary>
        ///Returns the state of the ring light.
        ///</summary>
        ///<returns>State of the ring light.</returns>
        public bool GetState()
        {
            return state;
        }

        ///<summary>
        ///Sets the uniform color of the ring light. Sends the color over MQTT to the device.
        ///</summary>
        ///<param name="color">Unity Color object.</param>
        public void SetColor(Color color)
        {
            this.color = color;
            foreach (RingLightLed led in ledList)
            {
                led.SetColor(color);
            }
            byte[] colorBytes = new byte[] { (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255) };
            device.SendActionMessage("ringlight/color", colorBytes);
        }

        ///<summary>
        ///Sets the color of an individual LED on the ring light.
        ///</summary>
        ///<param name="color">Unity Color object.</param>
        ///<param name="i">Index of the LED to change in the LED list.</param>
        public void SetIndividualColor(Color color, int i)
        {
            if (i >= 0 && i < ledList.Count)
            {
                ledList[i].SetColor(color);
            }
        }

        ///<summary>
        ///Method to send all the individual LED colors over MQTT to the device.!-- 
        ///</summary>
        public void SendLedColorsToDevice()
        {
            List<byte> colorBytes = new List<byte>();
            foreach (RingLightLed led in ledList)
            {
                Color c = led.GetColor();
                colorBytes.Add((byte)(c.r * 255));
                colorBytes.Add((byte)(c.g * 255));
                colorBytes.Add((byte)(c.b * 255));
            }
            device.SendActionMessage("ringlight/all_colors", colorBytes.ToArray());
        }

        ///<summary>
        ///Gets the uniform color of the ring light.
        ///</summary>
        ///<returns>Unity Color object.</returns>
        public Color GetColor()
        {
            return color;
        }
        
        ///<summary>
        ///Sets the number of active LEDs for the ring light. Will restrict the number within 0 and max. Sends the number over MQTT to the device.
        ///</summary>
        ///<param name="numOfLeds">Number of leds to set as active.</param>
        public void SetNumOfLeds(int numOfLeds)
        {
            this.numOfLeds = Mathf.Min(Mathf.Max(numOfLeds, 0), maxNumLeds);
            UpdateLeds();
            device.SendActionMessage("ringlight/number_of_leds", this.numOfLeds);
        }

        ///<summary>
        ///Returns the number of active LEDs for this ring light.
        ///</summary>
        ///<returns>Integer of active LEDs.</returns>
        public int GetNumOfLeds()
        {
            return numOfLeds;
        }

        ///<summary>
        ///Returns the max number of LEDs for this ring light.
        ///</summary>
        ///<returns>Integer of the max number of LEDs.</returns>
        public int GetMaxNumLeds()
        {
            return maxNumLeds;
        }

        ///<summary>
        ///Returns the list of individual LEDs for this ring light.
        ///</summary>
        ///<returns>List of individual LEDs.</returns>
        public RingLightLed[] GetLedList()
        {
            return ledList.ToArray();
        }

        ///<summary>
        ///Not used now it seems
        ///</summary>
        private string BuildNumberString(int color)
        {
            string temp = "";
            if (color < 10)
            {
                temp += "00";
            }
            else if (color < 100)
            {
                temp += "0";
            }
            return temp += color;
        }

        ///<summary>
        ///Updates the individual LEDs based on the number of active LEDs and the general state of the ring light.
        ///</summary>
        private void UpdateLeds()
        {
            if (state)
            {
                for (int i = 0; i < numOfLeds; i++)
                {
                    ledList[i].SetState(true);
                }
                for (int i = numOfLeds; i < maxNumLeds; i++)
                {
                    ledList[i].SetState(false);
                }
            }
            else
            {
                for (int i = 0; i < maxNumLeds; i++)
                {
                    ledList[i].SetState(false);
                }
            }
        }
    }
}
