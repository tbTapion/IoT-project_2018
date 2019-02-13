using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingLight : DeviceComponent{

	Color color;
	bool state;
	
	public RingLight(TwinObject device){
		color = new Color(0,0,0);
	}

	public override void update(){

	}

	public void setState(bool state){
		this.state = state;
		device.sendActionMessage("ringlight", (state ? 1 : 0).ToString());
	}
}
