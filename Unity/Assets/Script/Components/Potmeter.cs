using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potmeter : MonoBehaviour {

    private int value;

	// Use this for initialization
	void Start () {
        value = 0;
	}
	
	// Update is called once per frame
	void Update () {
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
