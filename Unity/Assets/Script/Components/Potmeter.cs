using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potmeter : DeviceComponent{

    private int value;

    private void Start()
    {
        value = 0;
    }

    public void SetValue(int value)
    {
        this.value = value;
    }

    public int GetValue()
    {
        return value;
    }

}
