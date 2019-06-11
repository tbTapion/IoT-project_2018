using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework.Component.Examples
{
    public class RingLightLed : MonoBehaviour
    {
        private bool state;
        private Color color;

        public void Toggle()
        {
            SetState(!state);
        }

        public void SetState(bool state)
        {
            this.state = state;
            UpdateTransformColor();
        }

        public bool GetState()
        {
            return state;
        }

        public void SetColor(Color color)
        {
            this.color = color;
            UpdateTransformColor();
        }

        public Color GetColor()
        {
            return color;
        }

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