using System.Collections;
using System.Collections.Generic;
using ExactFramework.Configuration;
using UnityEngine;

namespace KeepAliveExample
{
    public class Red : Tile
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            configName = "redtile";
            ringLight.Init(12);
        }
    }
}
