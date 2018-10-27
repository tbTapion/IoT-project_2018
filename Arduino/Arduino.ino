/*
Arduino ESP8266 WiFi and MQTT
*/

#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include "Led.h"
#include "PLab_PushButton.h"

// Update these with values suitable for your network.

const char* ssid = "your ssid";
const char* password = "your pass";
const char* mqtt_server = "your ip";
const int mqtt_port = 1883;

WiFiClient espClient;
PubSubClient client(espClient);

Led led(BUILTIN_LED); //Built-in led as output
PLab_PushButton button(4); //Button class with pin nr. 4 passed

const char* clientID; //Filled with mac address
String clientIDstr;


void setup() {
  Serial.begin(115200);
  setup_wifi();
  client.setServer(mqtt_server, mqtt_port);
  client.setCallback(callback);
}

void setup_wifi() {
  delay(10);
  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  clientIDstr = WiFi.macAddress();
  clientID = clientIDstr.c_str();
  Serial.println(clientID);
}

void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");

  char* topicElement;
  topicElement = strtok(topic, "/");
  while(topicElement != NULL){
    if(topicElement == "ping"){
      ping_event();
    }else if(topicElement == "action"){
      action_event(topicElement, payload);
    }else if(topicElement == "get"){
      get_event(topicElement);
    }else if(topicElement == "getconfig"){
      getconfig_event();
    }
    topicElement = strtok(NULL, "/");
  }
}

void getconfig_event(){
  Serial.print("Get configuration event received!");
  Serial.print("Sending active devices!");
  client.publish(("unity/device/" + clientIDstr + "/config/cube1").c_str(), "1");
}

void get_event(char* topicElement){
  while(topicElement != NULL){
    if(topicElement == "led"){
      client.publish(("unity/device/" + clientIDstr + "/value/led").c_str(), (char*)led.getValue());
    }else if(topicElement == "button"){
      client.publish(("unity/device/" + clientIDstr + "/value/button").c_str(),(char*)button.isDown());
    }
    topicElement = strtok(NULL, "/");
  }
}

void action_event(char* topicElement, byte* payload){
  while(topicElement != NULL){
    if(topicElement == "led"){
        if ((char)payload[0] == '1') {
          led.setValue(HIGH);
        }else{
          led.setValue(LOW);
        }
    }
    topicElement = strtok(NULL, "/");
  }
}

void ping_event(){
  Serial.print("Ping event received!");
  Serial.print("Pinging back!");
  client.publish(("unity/device/" + clientIDstr + "/ping").c_str(), "1");
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Attempt to connect
    if (client.connect("ESP8266Client")) {
      Serial.println("connected");
      // Once connected, publish an announcement to unity: unity/connect/device-id
      client.publish(("unity/connect/"+clientIDstr).c_str(), "1");
      // Then Subcribe to everything client-id/#
      client.subscribe((clientIDstr + "/#").c_str());
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}

void loop() {
  if (!client.connected()) {
    reconnect();
  }
  client.loop();
  if(button.justpressed()){
    client.publish(("unity/device/" + clientIDstr + "/event/button").c_str(), "1");
  }else if(button.justreleased()){
    client.publish(("unity/device/" + clientIDstr + "/event/button").c_str(), "0");
  }
}
