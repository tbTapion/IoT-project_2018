using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : DeviceComponent {

    private bool state;
	private bool _justPressed;
	private bool _justReleased;

	public Button(TwinObject device){
		this.device = device;
		state = false;
	}
	
	public override void update () {
		_justPressed = false;
		_justReleased = false;
	}

	public bool justPressed(){
		return _justPressed;
	}

	public bool justReleased(){
		return _justReleased;
	}

    public bool ispressed()
    {
        return state;
    }

    public void setPressed(bool pressed)
    {
        state = pressed;
		if (state) {
			_justPressed = true;
		} else {
			_justReleased = true;
		}
    }
}
