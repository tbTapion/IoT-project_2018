using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1 : TwinObject {

    private Button button;
    private Potmeter potmeter;

	// Use this for initialization
	void Start () {
        configName = "cube1";
        button = gameObject.AddComponent<Button>();
        potmeter = gameObject.AddComponent<Potmeter>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
