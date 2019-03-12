using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCourseTile : RedTile
{
    public bool active;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void setActive(bool active){
        this.active = active;
        ringLight.setState(active);
    }

    public void setupWaiting(){
        ringLight.setColor(Color.red);
        System.Threading.Thread.Sleep(100);
        setActive(true);
    }

    public void setupReady(){
        ringLight.setColor(Color.green);
        setActive(false);
    }
}
