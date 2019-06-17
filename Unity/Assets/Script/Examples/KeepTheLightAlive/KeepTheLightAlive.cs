using System.Collections;
using System.Collections.Generic;
using ExactFramework.Handlers;
using UnityEngine;

namespace KeepAliveExample
{
    public class KeepTheLightAlive : MonoBehaviour
    {

        public enum GameStates : int { SETUP, PLAY, GAMEOVER };
        public GameStates gameState;

        List<Tile> tileList;

        private MQTTHandler mqttHandler;

        private int gameOverCountDown = 4;
        private int gameOverTime = 60 * 5;

        // Start is called before the first frame update
        void Start()
        {
            //MQTT handler. Takes care of the connection to the RPI and sending/receiving messages.
            mqttHandler = new MQTTHandler("129.241.104.251"); //Enter IP here
            // Game-stuff
            tileList = new List<Tile>();
            for (int i = 0; i < 2; i++)
            {
                GameObject redObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + (i * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
                Red redTile = redObj.AddComponent<Red>();
                redObj.name = "RedTile" + i;
                tileList.Add(redTile);
                mqttHandler.AddTwinObject(redTile);

                GameObject blueObj = Instantiate(Resources.Load("Prefabs/TilePrefab"), new Vector3(-1.6f + ((2 + i) * 1.05f), 0.0f, 0.0f), Quaternion.identity) as GameObject;
                Blue blueTile = blueObj.AddComponent<Blue>();
                blueObj.name = "BlueTile" + i;
                tileList.Add(blueTile);
                mqttHandler.AddTwinObject(blueTile);
            }

        }

        // Update is called once per frame
        void Update()
        {
            mqttHandler.Update(); //MQTT handler's update function. Handles updating all the objects based on incoming messages.
            if (mqttHandler.AllDevicesConnected()) //Checks to see if all devices are connected.
            {
                if (gameState == GameStates.SETUP)
                {
                    SetupAndPickTile();
                }
                else if (gameState == GameStates.GAMEOVER)
                {
                }
            }
        }

        void SetupAndPickTile()
        {
            foreach (Tile tile in tileList)
            {
                tile.SetOtherTiles(tileList);
            }
            tileList[Random.Range(0, tileList.Count)].SetActive();
            gameState = GameStates.PLAY;
        }

        void GameOver()
        {
            if (gameOverCountDown > 0)
            {
                gameOverTime--;
                if(gameOverTime <=0){
                    gameOverCountDown--;
                    gameOverTime = 5*60;
                    foreach (Tile tile in tileList)
                    {
                        tile.ToggleLEDs();
                    }
                }
            }else{
                gameOverCountDown = 4;
                gameState = GameStates.SETUP;
            }
        }

        public void SetGameStatePlay()
        {
            gameState = GameStates.PLAY;
        }

        public void SetGameStateSetup()
        {
            gameState = GameStates.SETUP;
        }

        public void SetGameStateGameOver()
        {
            gameState = GameStates.GAMEOVER;
        }
    }
}

