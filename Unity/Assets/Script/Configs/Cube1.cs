using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1 : TwinObject {

    private Led led;

	// Use this for initialization
	public override void Start () {
		base.Start();
        configName = "cube1";
        led = new Led(this);
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	protected override void updateComponent(string component, string payload){
		if (component == "led") {
			if (payload == "1") {
				led.setState (true);
			} else {
				led.setState (false);
			}
		}
	}

    public Led getLed()
    {
        return led;
    }
}
