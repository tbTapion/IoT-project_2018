using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube3 : TwinObject {

	protected Led led;
	protected Button button;

	// Use this for initialization
	private void Start () {
		configName = "cube3";
        button = gameObject.AddComponent<Button>();
        led = gameObject.AddComponent<Led>();
	}
	
	// Update is called once per frame
	private void Update () {
		if(led.GetState()){
			//GetComponent<Renderer>().material.shader = Shader.Find("_Color");
			GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
		}
        else
        {
            //GetComponent<Renderer>().material.shader = Shader.Find("_Color");
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
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
