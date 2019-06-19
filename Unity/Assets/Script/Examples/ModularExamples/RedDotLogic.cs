using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameLogicBase))]
public class RedDotLogic : MonoBehaviour
{
    GameLogicBase gameLogicBase;
    List<RedDotBehaviour> redDotDevices = new List<RedDotBehaviour>();

    // Start is called before the first frame update
    void Start()
    {
        gameLogicBase = GetComponent<GameLogicBase>();
        redDotDevices = gameLogicBase.GetDeviceWithBehavior<RedDotBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
