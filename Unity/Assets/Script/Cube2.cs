﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : TwinObject {

    private Button button;
    private Potmeter potmeter;

	// Use this for initialization
	public override void Start () {
		base.Start ();
        configName = "cube1";
        button = gameObject.AddComponent<Button>();
        potmeter = gameObject.AddComponent<Potmeter>();
	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	protected override void updateComponent(string component, string payload){
		if (component == "button") {
			if (payload == "1") {
				button.setPressed (true);
			} else {
				button.setPressed (false);
			}
		} else if (component == "potmeter") {
			int parsedValue = -1;
			try{
				int.TryParse (payload, out parsedValue);
			}catch(System.Exception e){
				Debug.Log(e.Message);
			}
			if (parsedValue != -1) {
				potmeter.setValue (parsedValue);
			}
		}
	}
}
