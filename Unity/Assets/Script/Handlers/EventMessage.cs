using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMessage
{
    public string name;
    public string component;
    public byte[] payload;
    public bool state;
    public int value;

    public EventMessage(string component, string name, byte[] payload)
    {
        this.component = component;
        this.name = name;
        switch (component)
        {
            case "led":
                handleLed(name, payload);
                break;
            case "button":
                handleButton(name, payload);
                break;
            case "potmeter":
                handlePotmeter(name, payload);
                break;
            case "ringlight":
                handleRingLight(name, payload);
                break;
            case "timeofflight":
                handleTimeOfFlight(name, payload);
                break;
            case "imu":
                handleIMU(name, payload);
                break;
        }
    }

    private void parsevalue(byte[] payload)
    {
        int parsedValue = 0;
        for(int i = 0; i<payload.Length; i++){
            Debug.Log("payload value " + (int)payload[i]);
            parsedValue += (int)payload[i] * (int)Mathf.Pow(256, i);
        }
        value = parsedValue;
    }

    private void parsestate(byte[] payload)
    {
        if(payload[0] == 1){
            state = true;
        }else{
            state = false;
        }
    }

    private void handleLed(string type, byte[] payload)
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

    private void handleButton(string type, byte[] payload)
    {
        if (type == "state")
        {
            parsestate(payload);
        }
    }

    private void handlePotmeter(string type, byte[] payload)
    {
        if (type == "value")
        {
            parsevalue(payload);
        }
    }

    private void handleRingLight(string type, byte[] payload)
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

    private void handleTimeOfFlight(string type, byte[] payload)
    {   
        if(type == "value"){
            parsevalue(payload);
            state = true;
        }else if(type == "off"){
            state = false;
        }
    }

    private void handleIMU(string type, byte[] payload){
        if(type == "rotation"){
            this.payload = payload;
        }else if(type == "tapped"){
            parsestate(payload);
        }
    }
}
