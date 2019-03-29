using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DeviceComponent))]
public class RingLight : DeviceComponent
{

    private Color color;

    private bool state;

    private int maxNumLeds;
    private int numOfLeds;

    private List<RingLightLed> ledList;

    public override void Start()
    {
        base.Start();
        color = new Color(0, 1f, 0);
        ledList = new List<RingLightLed>();
    }

    public void Init(int numberOfLeds)
    {
        if (transform != null)
        {
            RingLightLed[] ledList = transform.Find("RingLight").GetComponentsInChildren<RingLightLed>();
            if (ledList != null)
            {
                this.ledList.AddRange(ledList);
            }
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

    public void Toggle()
    {
        SetState(!state);
    }

    public void SetState(bool state)
    {
        this.state = state;
        if (ledList != null)
        {
            UpdateLeds();
        }
        device.SendActionMessage("ringlight/state", state);
    }

    public bool GetState()
    {
        return state;
    }

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

    public void SetIndividualColor(Color color, int i)
    {
        if (i >= 0 && i < ledList.Count)
        {
            ledList[i].SetColor(color);
        }
    }

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

    public Color GetColor()
    {
        return color;
    }

    public void SetNumOfLeds(int numOfLeds)
    {
        this.numOfLeds = Mathf.Min(Mathf.Max(numOfLeds,0), maxNumLeds);
        UpdateLeds();
        device.SendActionMessage("ringlight/number_of_leds", this.numOfLeds);
    }

    public int GetNumOfLeds()
    {
        return numOfLeds;
    }

    public int GetMaxNumLeds()
    {
        return maxNumLeds;
    }

    public RingLightLed[] GetLedList()
    {
        return ledList.ToArray();
    }

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
        }else{
            for (int i = 0; i < maxNumLeds; i++)
            {
                ledList[i].SetState(false);
            }
        }
    }
}
