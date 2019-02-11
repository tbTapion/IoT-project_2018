using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube3 : TwinObject {

	private Led led;
	private Button button;

	// Use this for initialization
	public override void Start () {
		base.Start();
		configName = "cube3";
		button = new Button(this);
		led = new Led(this);
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
        button.update();
	}

	protected override void updateComponent(string component, string payload){
		if (component == "button") {
			if (payload == "1") {
				button.setPressed (true);
			} else {
				button.setPressed (false);
			}
		}else if (component == "led") {
			if (payload == "1") {
				led.setState (true);
			} else {
				led.setState (false);
			}
		}
	}

	public Led getLed(){
		return led;
	}

	public Button getButton(){
		return button;
	}
}
