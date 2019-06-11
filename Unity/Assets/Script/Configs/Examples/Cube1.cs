using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using UnityEngine;

namespace ExactFramework.Configuration.Examples{
	public class Cube1 : TwinObject {

		protected Led led;

		// Use this for initialization
		protected override void Start () {
			configName = "cube1";
			led = AddDeviceComponent<Led>("led");
		}
	}
}
