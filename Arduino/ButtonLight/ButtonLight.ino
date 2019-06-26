/*
Arduino ESP8266 WiFi and MQTT
*/

#include "MQTTSocket.h"
#include "PLab_PushButton.h"
#include "Led.h"

// Update these with values suitable for your network.

const char* ssid = "MyExactNet";
const char* password = "MyExactNetPassword";
const char* mqtt_server = "192.168.42.1";
const int mqtt_port = 1883;
const char* configID = "ButtonLight";
const char* deviceName = "switch";

//MQTT and WIFi class
MQTTSocket mqttSocket;

//Component classes
Led led(BUILTIN_LED); //Built-in led as output
PLab_PushButton button(4); //Button object

//Setup
void setup() {
  Serial.begin(115200); //Starts serial
  mqttSocket.initConfiguration(configID, deviceName); //Sets up the configuration variables
  mqttSocket.initWiFi(ssid, password); //Connects to the WiFi with SSID and Password specified
  mqttSocket.initMQTT(mqtt_server); //
  mqttSocket.setCallbackGet(getEvent);
  mqttSocket.setCallbackAction(actionEvent);
  mqttSocket.setCallbackHello(helloEvent);
}

//Called when unity sends the unityconnect/hello message
//Useful to reset output components
void helloEvent(){
  led.setHeartbeatInterval(0);
  led.setState(LOW);
}

//Called on get message received and when the main callback function handles the message
void getEvent(char *component, char *valueType){
  if(strcmp(component, "led") == 0){
    if(strcmp(valueType, "state") == 0){
      string state = "" + char(led.getState());
      mqttSocket.sendValue("led", "state", state);
    }
  }else if(strcmp(component, "button") == 0){
    if(strcmp(valueType, "state") == 0){
      string state = "" + char(button.isDown());
      mqttSocket.sendValue("button", "state", state);
    }
  }
}

//Called on action message received and when the main callback function handles the message
void actionEvent(char *component, char *actionType, byte *payload){
  if(strcmp(component, "led") == 0){
    if(strcmp(actionType, "state") == 0){
      if(payload[0] == "1"){
        led.setState(HIGH);
      }else{
        led.setState(LOW);
      }
    }
  }
}

//Main arduino loop.
void loop() {
  mqttSocket.loop();
  button.update();
  led.update();
  if(button.pressed()){
    mqttSocket.sendEvent("button", "pressed", "1");
  }else if(button.released()){
    mqttSocket.sendEvent("button", "released", "1");
  }
}
