using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingLight : DeviceComponent
{

    private Color color;

    private bool state;

    private int maxNumLeds;
    private int numOfLeds;

    private List<RingLightLed> ledList;

    public RingLight(TwinObject device, int numOfLeds, Transform transform)
    {
        this.device = device;
        color = new Color(0, 0, 0);
        this.numOfLeds = numOfLeds;
        maxNumLeds = numOfLeds;
        ledList = new List<RingLightLed>();
        if (transform != null)
        {
            RingLightLed[] ledList = transform.Find("RingLight").GetComponentsInChildren<RingLightLed>();
            Debug.Log(ledList[0]);
            if (ledList != null)
            {
                Debug.Log("Found leds");
                foreach (RingLightLed led in ledList)
                {
                    this.ledList.Add(led);
                    led.setColor(color);
                }
            }
        }
        else
        {
            for (int i = 0; i < numOfLeds; i++)
            {
                this.ledList.Add(new RingLightLed());
            }
        }
    }

    public override void update()
    {
    }

    public void toggle()
    {
        setState(!state);
    }

    public void setState(bool state)
    {
        this.state = state;
        if (ledList != null)
        {
            updateLeds();
        }
        device.sendActionMessage("ringlight/state", state);
    }

    public bool getState()
    {
        return state;
    }

    public void setColor(Color color)
    {
        this.color = color;
        foreach (RingLightLed led in ledList)
        {
            led.setColor(color);
        }
        byte[] colorBytes = new byte[] { (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255) };
        device.sendActionMessage("ringlight/color", colorBytes);
    }

    public void setColor(Color color, int i)
    {
        if (i >= 0 && i < ledList.Count)
        {
            ledList[i].setColor(color);
        }
    }

    public void setAllLedsColor()
    {
        List<byte> colorBytes = new List<byte>();
        foreach (RingLightLed led in ledList)
        {
            Color c = led.getColor();
            colorBytes.Add((byte)(c.r * 255));
            colorBytes.Add((byte)(c.g * 255));
            colorBytes.Add((byte)(c.b * 255));
        }
        device.sendActionMessage("ringlight/all_colors", colorBytes.ToArray());
    }

    public Color getColor()
    {
        return color;
    }

    public void setNumOfLeds(int numOfLeds)
    {
        this.numOfLeds = numOfLeds;
        updateLeds();
        device.sendActionMessage("ringlight/number_of_leds", numOfLeds);
    }

    public int getNumOfLeds()
    {
        return numOfLeds;
    }

    public RingLightLed[] getLedList()
    {
        return ledList.ToArray();
    }

    public int getMaxNumLeds()
    {
        return maxNumLeds;
    }

    private string buildNumberString(int color)
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

    private void updateLeds()
    {
        if (state)
        {
            for (int i = 0; i < numOfLeds; i++)
            {
                ledList[i].setState(true);
            }
            for (int i = numOfLeds; i < maxNumLeds; i++)
            {
                ledList[i].setState(false);
            }
        }else{
            for (int i = 0; i < maxNumLeds; i++)
            {
                ledList[i].setState(false);
            }
        }
    }
}
