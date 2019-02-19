using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Cube3
{   

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        configName = "tile";
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    protected override void updateComponent(string component, string payload)
    {
        if(component == "timeofflight"){
			int parsedValue = -1;
			try{
				int.TryParse (payload, out parsedValue);
			}catch(System.Exception e){
				Debug.Log(e.Message);
            }
			if (parsedValue != -1) {
				    timeOfFlight.setDistance(parsedValue);
            }
        }
    }
}
