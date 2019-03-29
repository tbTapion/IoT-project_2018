using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube3 : TwinObject {

	protected Led led;
	protected Button button;

	// Use this for initialization
    protected override void Start () {
		configName = "cube3";
        button = gameObject.AddComponent<Button>();
        led = gameObject.AddComponent<Led>();
	}

	protected override void UpdateComponent(EventMessage e){
		if (e.component == "button") {
            if (e.state)
            {
                SendMessage("OnButtonPressed");
            }
            else
            {
                SendMessage("OnButtonReleased");
            }
		}else if (e.component == "led") {
			if(e.name == "state"){
				led.SetState(e.state);
			}else if(e.name == "heartbeat"){
				led.SetHeartbeatTime(e.value);
			}
		}
	}
}
