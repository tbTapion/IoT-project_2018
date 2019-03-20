using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingLightLed : MonoBehaviour
{
    private bool state;
    private Color color;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void toggle()
    {
        setState(!state);
    }

    public void setState(bool state)
    {
        this.state = state;
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            if (state)
            {
                r.material.SetColor("_Color", Color.red);
            }
            else
            {
                r.material.SetColor("_Color", Color.black);
            }
        }
    }

    public bool getState()
    {
        return state;
    }

    public void setColor(Color color)
    {
        this.color = color;
    }

    public Color getColor()
    {
        return color;
    }
}
