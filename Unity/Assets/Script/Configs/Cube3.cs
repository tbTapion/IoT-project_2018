using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube3 : TwinObject {

	//private Led led;
	//private Button button;

	private RingLight ringLight;
	private TimeOfFlight tof;

	// Use this for initialization
	public override void Start () {
		base.Start();
		configName = "cube3";
		//button = new Button(this);
		//led = new Led(this);
		ringLight = new RingLight(this);
		tof = new TimeOfFlight(this);

	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		if(ringLight.getState()){
			//GetComponent<Renderer>().material.shader = Shader.Find("_Color");
			GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
		}
        else
        {
            //GetComponent<Renderer>().material.shader = Shader.Find("_Color");
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
	}

	protected override void updateComponent(string component, string payload){
		if (component == "timeofflight") {
			int parsedValue = -1;
			try{
				int.TryParse (payload, out parsedValue);
			}catch(System.Exception e){
				Debug.Log(e.Message);
			}
			if (parsedValue != -1) {
				tof.setValue (parsedValue);
			}
		}else if (component == "ringlight") {
			if (payload == "1") {
				ringLight.setState (true);
			} else {
				ringLight.setState (false);
			}
		}
	}

	public RingLight getRingLight(){
		return ringLight;
	}

	public TimeOfFlight getTimeOfFlight(){
		return tof;
	}
}
