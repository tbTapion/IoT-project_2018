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
                HandleLed(name, payload);
                break;
            case "button":
                HandleButton(name, payload);
                break;
            case "potmeter":
                HandlePotmeter(name, payload);
                break;
            case "ringlight":
                HandleRingLight(name, payload);
                break;
            case "timeofflight":
                HandleTimeOfFlight(name, payload);
                break;
            case "imu":
                HandleIMU(name, payload);
                break;
        }
    }

    private void Parsevalue(byte[] payload)
    {
        int parsedValue = 0;
        for(int i = 0; i<payload.Length; i++){
            parsedValue += (int)payload[i] * (int)Mathf.Pow(256, i);
        }
        value = parsedValue;
    }

    private void Parsestate(byte[] payload)
    {
        if(payload[0] == 1){
            state = true;
        }else{
            state = false;
        }
    }

    private void HandleLed(string type, byte[] payload)
    {
        if (type == "state")
        {
            Parsestate(payload);
        }
        else if (type == "heartbeat")
        {
            Parsevalue(payload);
        }
    }

    private void HandleButton(string type, byte[] payload)
    {
        if (type == "state")
        {
            Parsestate(payload);
        }
    }

    private void HandlePotmeter(string type, byte[] payload)
    {
        if (type == "value")
        {
            Parsevalue(payload);
        }
    }

    private void HandleRingLight(string type, byte[] payload)
    {
        if (type == "state")
        {
            Parsestate(payload);
        }
        else if (type == "color")
        {
            this.payload = payload;
        }
        else if (type == "numOfLeds")
        {
            Parsevalue(payload);
        }
    }

    private void HandleTimeOfFlight(string type, byte[] payload)
    {   
        if(type == "value"){
            Parsevalue(payload);
            state = true;
        }else if(type == "off"){
            state = false;
        }
    }

    private void HandleIMU(string type, byte[] payload){
        if(type == "rotation"){
            this.payload = payload;
        }else if(type == "tapped"){
            Parsestate(payload);
        }
    }
}
