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
		device.sendActionMessage("ringlight/state", state);
	}

	public bool getState(){
		return state;
	}

	public void setColor(Color color){
		this.color = color;
		byte[] colorBytes = new byte[]{(byte)(color.r*255),(byte)(color.g*255),(byte)(color.b*255)};
		device.sendActionMessage("ringlight/color", colorBytes);
	}

	public void setAllLedsColor(){
		List<byte> colorBytes = new List<byte>();
		foreach(RingLightLed led in ledList){
			Color c;
			if(led.getState()){
				c = led.getColor();
			}else{
				c = Color.black;
			}
			byte[] colors = new byte[]{(byte)(c.r*256),(byte)(c.g*256),(byte)(c.b*256)};
			colorBytes.AddRange(colors);
		}
		device.sendActionMessage("ringlight/all_colors", colorBytes.ToArray());
	}

	public Color getColor(){
		return color;
	}

	public void setNumOfLeds(int numOfLeds){
		this.numOfLeds = numOfLeds;
		device.sendActionMessage("ringlight/number_of_leds", numOfLeds);
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
