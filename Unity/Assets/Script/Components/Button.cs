using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    private bool state;
	private bool _justPressed;
	private bool _justReleased;

	// Use this for initialization
	void Start () {
        state = false;
	}
	
	// Update is called once per frame
	void Update () {
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
