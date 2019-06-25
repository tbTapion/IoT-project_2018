using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBlue : MyTile
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        configName = "bluetile";
    }
}
