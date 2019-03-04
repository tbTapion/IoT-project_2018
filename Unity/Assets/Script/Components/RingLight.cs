using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingLight : DeviceComponent{

	private Color color;

	private bool state;

	private int numOfLeds;
	
	private RingLightLed[] ledList;

	public RingLight(TwinObject device, RingLightLed[] ledList){
		color = new Color(0,0,0);
		this.ledList = ledList;
		numOfLeds = 12;
	}

	public override void update(){
	}

	public void toggle()
    {
        setState(!state);
    }

	public void setState(bool state){
		this.state = state;
		if(state == false){
			for(int i = 0; i<ledList.Length; i++){
				ledList[i].setState(false);
			}
		}else{
			for(int i = 0; i<numOfLeds; i++){
				ledList[i].setState(state);
			}
		}
		//device.sendActionMessage("ringlight/state", (state ? 1 : 0).ToString());
	}

	public bool getState(){
		return state;
	}

	public void setColor(Color color){
		this.color = color;
		string colorString = buildNumberString((int)(color.r * 255)) + "-" + 
			buildNumberString((int)(color.g * 255)) +"-" + 
			buildNumberString((int)(color.b * 255));
		Debug.Log(colorString);
		//device.sendActionMessage("ringlight/color", colorString);
	}

	public Color getColor(){
		return color;
	}

	public void setNumOfLeds(int numOfLeds){
		this.numOfLeds = numOfLeds;
	}

	public int getNumOfLeds(){
		return numOfLeds;
	}

	public RingLightLed[] getLedList(){
		return ledList;
	}

	private string buildNumberString(int color){
		string temp = "";
		if(color< 10){
			temp += "00";
		}else if(color < 100) {
			temp += "0";
		}
		return temp += color;
	}
}
