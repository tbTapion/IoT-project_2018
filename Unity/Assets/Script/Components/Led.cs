using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Led : MonoBehaviour {

    protected bool state;
    protected bool heartbeat;
    protected int heartbeatTime;
    protected TwinObject obj;

	// Use this for initialization
	public virtual void Start () {
        state = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setDevice(TwinObject obj)
    {
        this.obj = obj;
    }

    public void toggle()
    {
        state = !state;
    }

    public void setState(bool state)
    {
        this.state = state;
    }
}
