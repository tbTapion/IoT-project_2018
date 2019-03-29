using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : TwinObject {

    protected Button button;
    protected Potmeter potmeter;


	// Use this for initialization
	protected override void Start () {
        base.Start();
        configName = "cube2";
        button = gameObject.AddComponent<Button>();
        potmeter = gameObject.AddComponent<Potmeter>();
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
