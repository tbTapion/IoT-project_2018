using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : TwinObject {

    private Button button;
    private Potmeter potmeter;


	// Use this for initialization
	public override void Start () {
        base.Start();
        configName = "cube2";
        button = new Button(this);
        potmeter = new Potmeter(this);
	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();
		button.update();
	}

	protected override void updateComponent(EventMessage e){
		if (e.component == "button") {
			button.setPressed(e.state);
		} else if (e.component == "potmeter") {
			potmeter.setValue(e.value);
		}
	}

    public Button getButton()
    {
        return button;
    }

    public Potmeter getPotmeter()
    {
        return potmeter;
    }
}
