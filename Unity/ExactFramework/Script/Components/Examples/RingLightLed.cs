using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples
{
    ///<summary>
    ///Digital representation of a ring light LED. Kept as a monobehaviour subclass incase it's put on a game object.
    ///</summary>
    public class RingLightLed : MonoBehaviour
    {
        ///<summary>
        ///State of the LED.
        ///</summary>
        private bool state;
        ///<summary>
        ///Color of the LED, if it's an rgb ring light.
        ///</summary>
        private Color color;
        
        ///<summary>
        ///Method to toggle the LED's state between on and off.
        ///Calls the SetState method.
        ///</summary>
        public void Toggle()
        {
            SetState(!state);
        }

        ///<summary>
        ///Sets the state of the LED.
        ///</summary>
        ///<param name="state">Boolean state to update to.</param>
        public void SetState(bool state)
        {
            this.state = state;
            UpdateTransformColor();
        }

        ///<summary>
        ///Returns the current state of the LED.
        ///</summary>
        ///<returns>Boolean state of the LED</returns>
        public bool GetState()
        {
            return state;
        }

        ///<summary>
        ///Sets the color of the LED.
        ///</summary>
        ///<param name="color">Color to set on the LED.</param>
        public void SetColor(Color color)
        {
            this.color = color;
            UpdateTransformColor();
        }

        ///<summary>
        ///Returns the current color of the LED.
        ///</summary>
        ///<returns>Color of the LED as a Unity Color variable.</returns>
        public Color GetColor()
        {
            return color;
        }

        ///<summary>
        ///Updates the transform the class is connected to.
        ///Only 3D supported now, 2D will be supported soon.
        ///</summary>
        private void UpdateTransformColor()
        {
            Renderer r = GetComponent<Renderer>();
            if (r != null)
            {
                if (state)
                {
                    r.material.SetColor("_Color", color);
                }
                else
                {
                    r.material.SetColor("_Color", Color.clear);
                }
            }
        }
    }
}