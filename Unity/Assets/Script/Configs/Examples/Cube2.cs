using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using UnityEngine;

namespace ExactFramework.Configuration.Examples{
    ///<summary>
    ///Digital representation of a device wuth a simple button and potmeter connected.
    ///</summary>
    public class Cube2 : TwinObject {

        protected Button button;
        protected Potmeter potmeter;


        // Use this for initialization
        protected override void Start () {
            base.Start();
            configName = "cube2";
            button = AddDeviceComponent<Button>("button");
            potmeter = AddDeviceComponent<Potmeter>("potmeter");
        }
    }
}
