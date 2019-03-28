using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : TwinObject {

    protected Button button;
    protected Potmeter potmeter;


	// Use this for initialization
	private void Start () {
        configName = "cube2";
        button = gameObject.AddComponent<Button>();
        potmeter = gameObject.AddComponent<Potmeter>();
	}

	// Update is called once per frame
	private void Update () {
	}

	protected override void UpdateComponent(EventMessage e){
		if (e.component == "button") {
            if (e.state)
            {
                SendMessage("OnButtonPressed");
            }
            else
            {
                SendMessage("OnButtonReleased");
            }
		} else if (e.component == "potmeter") {
			potmeter.SetValue(e.value);
		}
	}
}
