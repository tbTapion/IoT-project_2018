/*
Arduino ESP8266 WiFi and MQTT
*/

#include "MQTTSocket.h"
#include "PLab_PushButton.h"
#include "Potmeter.h"

// Update these with values suitable for your network.

const char* ssid = "MyExactNet";
const char* password = "MyExactNetPassword";
const char* mqtt_server = "192.168.4.1";
const int mqtt_port = 1883;
const char* configID = "cube2";
const char* deviceName = "empty";

//MQTT and WiFi class
MQTTSocket mqttSocket;

//Component classes
PLab_PushButton button(4); //Button class with pin nr. 4 passed. Change to actual pin used.
Potmeter potmeter(A0); //Potmeter class with pin nr. 5 passed. Change to actual pin used.

//Setup
void setup() {
  Serial.begin(115200);
  mqttSocket.initConfiguration(configID, deviceName);
  mqttSocket.initWiFi(ssid, password);
  mqttSocket.initMQTT(mqtt_server);
  mqttSocket.setCallbackGet(getEvent);
}

void getEvent(char *component, char *valueType){
  if(strcmp(component, "button") == 0){
    if(strcmp(valueType, "state") == 0){
      string state = "" + char(button.isDown());
      mqttSocket.sendValue("button", "state", state);
    }
  }else if(strcmp(component, "potmeter") == 0){
    mqttSocket.sendValue("potmeter", "value", String(potmeter.getValue()).c_str());
  }
}

void loop() {
  mqttSocket.loop();
  button.update();
  if(button.pressed()){
    mqttSocket.sendEvent("button", "pressed", "1");
  }else if(button.released()){
    mqttSocket.sendEvent("button", "released", "1");
  }else if(potmeter.checkWait()){
    mqttSocket.sendEvent("potmeter", "value", String(potmeter.getValue()).c_str());
  }
}
