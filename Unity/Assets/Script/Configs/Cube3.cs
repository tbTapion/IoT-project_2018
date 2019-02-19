using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube3 : TwinObject {

	protected RingLight led;
	protected TimeOfFlight timeOfFlight;

	// Use this for initialization
	public override void Start () {
		base.Start();
		configName = "cube3";
		timeOfFlight = new TimeOfFlight(this);
		led = new RingLight(this);
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

	}

	protected override void updateComponent(string component, string payload){
	}

	public RingLight getLed(){
		return led;
	}

	public TimeOfFlight getButton(){
		return timeOfFlight;
	}
}
