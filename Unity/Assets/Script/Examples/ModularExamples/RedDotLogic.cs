using ExactFramework.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameLogicBase))]
public class RedDotLogic : MonoBehaviour
{
    GameLogicBase gameLogicBase;
    GameStates gameState;

    List<RedDotBehaviour> redDotDevices = new List<RedDotBehaviour>();
    RedDotBehaviour activeObject;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameStates.SETUP;
        gameLogicBase = GetComponent<GameLogicBase>();
        redDotDevices = gameLogicBase.GetDevicesWithBehavior<RedDotBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameLogicBase.allDevicesConnected)
        {
            switch (gameState)
            {
                case GameStates.SETUP:
                    SetupGame();
                    break;
                case GameStates.PLAY:
                    PlayGame();
                    break;
            }
        }
    }

    void PlayGame()
    {
        if (!activeObject.GetActive())
        {
            List<RedDotBehaviour> otherRedDotDevices = new List<RedDotBehaviour>(redDotDevices);
            otherRedDotDevices.Remove(activeObject);
            activeObject = otherRedDotDevices[Random.Range(0, otherRedDotDevices.Count)];
            activeObject.SetActive();
        }
    }

    void SetupGame()
    {
        activeObject = redDotDevices[Random.Range(0, redDotDevices.Count)];
        activeObject.SetActive();
        gameState = GameStates.PLAY;
    }
}
