/*
Arduino ESP8266 WiFi and MQTT
*/

#include "MQTTSocket.h"
#include "Led.h"

// Update these with values suitable for your network.

const char* ssid = "MyExactNet";
const char* password = "MyExactNetPassword";
const char* mqtt_server = "192.168.4.1";
const int mqtt_port = 1883;
const char* configID = "cube1";
const char* deviceNam = "empty";

//MQTT and WiFi class
MQTTSocket mqttSocket;

//Component classes
Led led(BUILTIN_LED); //Built-in led as output

//Setup
void setup() {
  Serial.begin(115200);
  mqttSocket.initConfiguration(configID, deviceName);
  mqttSocket.initWiFi(ssid, password);
  mqttSocket.initMQTT(mqtt_server);
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
  if(component == "led"){
    if(valueType == "state"){
      char state = (char)led.getState();
      mqttSocket.sendValue("led", "state", state);
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

//Main arduino loop
void loop() {
  mqttSocket.loop();
  led.update();
}
