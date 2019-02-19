using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMessage
{
    public string name;
    public string component;
    public string payload;
    public bool state;
    public int value;
    public EventMessage(string component, string payload)
    {
        string[] msgPair = component.Split('-');
        component = msgPair[0];
        name = msgPair[1];
        switch (msgPair[0])
        {
            case "led":
                handleLed(msgPair[1], payload);
                break;
            case "button":
                handleButton(msgPair[1], payload);
                break;
            case "potmeter":
                handlePotmeter(msgPair[1], payload);
                break;
            case "ringlight":
                handleRingLight(msgPair[1], payload);
                break;
            case "timeofflight":
                handleTimeOfFlight(msgPair[1], payload);
                break;
            case "toneplayer":
                handleTonePlayer(msgPair[1], payload);
                break;
        }
    }

    private void parsevalue(string payload)
    {
        int parsedValue = -1;
        try
        {
            int.TryParse(payload, out parsedValue);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        if (parsedValue != -1)
        {
            value = parsedValue;
        }
    }

    private void parsestate(string payload)
    {
        if (payload == "1")
        {
            state = true;
        }
        else
        {
            state = false;
        }
    }

    private void handleLed(string type, string payload)
    {
        if (type == "state")
        {
            parsestate(payload);
        }
        else if (type == "heartbeat")
        {
            parsevalue(payload);
        }
    }

    private void handleButton(string type, string payload)
    {
        if (type == "state")
        {
            parsestate(payload);
        }
    }

    private void handlePotmeter(string type, string payload)
    {
        if (type == "value")
        {
            parsevalue(payload);
        }
    }

    private void handleRingLight(string type, string payload)
    {
        if (type == "state")
        {
            parsestate(payload);
        }
        else if (type == "color")
        {
            this.payload = payload;
        }
        else if (type == "numOfLeds")
        {
            parsevalue(payload);
        }
    }

    private void handleTimeOfFlight(string type, string payload)
    {
        if (type == "value")
        {
            parsevalue(payload);
        }
    }

    private void handleTonePlayer(string type, string payload)
    {
        if (type == "frequency")
        {
            parsevalue(payload);
        }
        else if (type == "duration")
        {
            parsevalue(payload);
        }
    }
}
