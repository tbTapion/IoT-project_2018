/*
Arduino ESP8266 WiFi and MQTT
*/

#include "MQTTSocket.h"

// Update these with values suitable for your network.

const char* ssid = "MyExactNet";
const char* password = "MyExactNetPassword";
const char* mqtt_server = "192.168.42.1";
const int mqtt_port = 1883;
const char* configID = "ButtonLight";
const char* deviceName = "switch";

MQTTSocket mqttSocket;

Led led(BUILTIN_LED); //Built-in led as output
PLab_PushButton button(4); //PLab PushButton type button.

void setup() {
  Serial.begin(115200); //Starts serial
  mqttSocket.initConfiguration(configID, deviceName); //Sets up the configuration variables
  mqttSocket.initWiFi(ssid, password); //Connects to the WiFi with SSID and Password specified
  mqttSocket.initMQTT(mqtt_server); //
  mqttSocket.setCallbackGet(getEvent);
  mqttSocket.setCallbackAction(actionEvent);
  mqttSocket.setCallbackHello(helloEvent);
}

void helloEvent(){
  led.setState(LOW);
  led.setHeartbeatInterval(0);
}

void getEvent(char *component, char *valueType){
  if(component == "led"){
    if(valueType == "state"){
      char state = (char)led.getState();
      mqttSocket.sendValue("led", "state", state);
    }
  }
}

void actionEvent(char *component, char *actionType, byte *payload){
  if(component == "led"){
    if(actionType == "state"){
      if(payload[0] == "1"){
        led.setState(HIGH);
      }else{
        led.setState(LOW);
      }
    }
  }
}

void loop() {
  mqttSocket.loop();
  button.update();
  if(button.pressed()){
    mqttSocket.sendEvent("button", "pressed", "1");
  }else if(button.released()){
    mqttSocket.sendEvent("button", "pressed", "1");
  }
}
