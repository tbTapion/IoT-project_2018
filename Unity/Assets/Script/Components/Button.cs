using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    private bool state;

	// Use this for initialization
	void Start () {
        state = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

    bool ispressed()
    {
        return state;
    }

    void setPressed(bool pressed)
    {
        state = pressed;
    }
}
