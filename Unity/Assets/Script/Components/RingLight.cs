using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingLight : DeviceComponent
{

    private Color color;

    private bool state;

    private int numOfLeds;

    private List<RingLightLed> ledList;

    public RingLight(TwinObject device, Transform transform)
    {
        this.device = device;
        color = new Color(0, 0, 0);
        numOfLeds = 24;
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
            if (state == false)
            {
                for (int i = 0; i < ledList.Count; i++)
                {
                    ledList[i].setState(false);
                }
            }
            else
            {
                for (int i = 0; i < numOfLeds; i++)
                {
                    ledList[i].setState(state);
                }
            }
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
        foreach(RingLightLed led in ledList){
            led.setColor(color);
        }
        byte[] colorBytes = new byte[] { (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255) };
        device.sendActionMessage("ringlight/color", colorBytes);
    }

	public void setColor(Color color, int i){
		if(i>= 0 && i < ledList.Count){
			ledList[i].setColor(color);
		}
	}

    public void setAllLedsColor()
    {
        List<byte> colorBytes = new List<byte>();
        foreach (RingLightLed led in ledList)
        {
            Color c;
            if (led.getState())
            {
                c = led.getColor();
            }
            else
            {
                c = Color.black;
            }
            byte[] colors = new byte[] { (byte)(c.r * 256), (byte)(c.g * 256), (byte)(c.b * 256) };
            colorBytes.AddRange(colors);
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
}
