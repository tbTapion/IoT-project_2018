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

		if(led.getState()){
			//GetComponent<Renderer>().material.shader = Shader.Find("_Color");
			GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
		}
        else
        {
            //GetComponent<Renderer>().material.shader = Shader.Find("_Color");
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
	}

	protected override void updateComponent(EventMessage e){
		if (e.component == "button") {
			button.setPressed(e.state);
		}else if (e.component == "led") {
			if(e.name == "state"){
				led.setState(e.state);
			}else if(e.name == "heartbeat"){
				led.setHeartbeatTime(e.value);
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
