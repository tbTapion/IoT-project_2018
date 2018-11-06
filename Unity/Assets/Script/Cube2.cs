using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : TwinObject {

    private Led led;

	// Use this for initialization
	void Start () {
        configName = "cube2";
        led = this.gameObject.AddComponent<Led>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
