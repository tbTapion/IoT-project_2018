using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using ExactFramework.Configuration;
using UnityEngine;

namespace KeepAliveExample
{
    public class Blue : Tile
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            configName = "bluetile";
            ringLight.Init(24);
        }
    }
}
