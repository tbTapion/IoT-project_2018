using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TwinObject))]
public abstract class DeviceComponent : MonoBehaviour
{
    protected TwinObject device;

    public virtual void Start()
    {
        device = GetComponent<TwinObject>();
    }
}