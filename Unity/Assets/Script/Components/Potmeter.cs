using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potmeter : DeviceComponent{

    private int value;
    
    public Potmeter(TwinObject device){
        this.device = device;
        value = 0;
    }
	
	public override void update () {
	}

    public void setValue(int value)
    {
        this.value = value;
    }

    public int getValue()
    {
        return value;
    }

}
