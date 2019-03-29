using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1 : TwinObject {

    protected Led led;

	// Use this for initialization
	protected override void Start () {
        configName = "cube1";
        led = gameObject.AddComponent<Led>();
	}

	protected override void UpdateComponent(EventMessage e){
		if (e.component == "led") {
			if(e.name == "state"){
				led.SetState(e.state);
			}
		}
	}
}
