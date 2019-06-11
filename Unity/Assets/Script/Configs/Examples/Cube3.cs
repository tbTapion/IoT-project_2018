using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using UnityEngine;

namespace ExactFramework.Configuration.Examples{
	public class Cube3 : TwinObject {

		protected Led led;
		protected Button button;

		// Use this for initialization
		protected override void Start () {
			configName = "cube3";
			button = AddDeviceComponent<Button>("button");
			led = AddDeviceComponent<Led>("led");
		}
	}
}
