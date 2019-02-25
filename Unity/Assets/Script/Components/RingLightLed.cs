using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingLightLed : MonoBehaviour
{
    public bool state;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void toggle()
    {
        setState(!state);
    }

    public void setState(bool state)
    {
        this.state = state;
        if(state){
			GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
		}
        else
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
    }

    public bool getState(){
        return state;
    }
}
