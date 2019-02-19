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

	protected override void updateComponent(EventMessage e){
		if (e.component == "led") {
			if(e.name == "state"){
				led.setState(e.state);
			}
		}
	}

    public Led getLed()
    {
        return led;
    }
}
