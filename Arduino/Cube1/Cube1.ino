/*
Arduino ESP8266 WiFi and MQTT
*/

#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include "Led.h"

// Update these with values suitable for your network.

const char* ssid = "MyExactNet";
const char* password = "MyExactNetPassword";
const char* mqtt_server = "192.168.4.1";
const int mqtt_port = 1883;

WiFiClient espClient;
PubSubClient client(espClient);

Led led(BUILTIN_LED); //Built-in led as output

const char* clientID; //Filled with mac address, unused, but kept in case of future functionality requiring it.
String clientIDstr; //String containing mac address, used in conjunction with message building

String configID = "cube1";

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
    Serial.println(topicElement);
    if(strcmp(topicElement, "ping") == 0){
      Serial.println("Ping event space entered!");
      ping_event(payload);
    }else if(strcmp(topicElement,"action") == 0){
      Serial.println("ActionEvent!");
      action_event(topicElement, payload);
    }else if(strcmp(topicElement,"get") == 0){
      get_event(topicElement);
    }else if(strcmp(topicElement,"getconfig") == 0){
      //getconfig_event();
    }
    topicElement = strtok(NULL, "/");
  }
}

//void getconfig_event(){
//  Serial.print("Get configuration event received!");
//  Serial.print("Sending active devices!");
//  client.publish(("unity/device/" + clientIDstr + "/config/cube1").c_str(), "1");
//}

void get_event(char* topicElement){
  while(topicElement != NULL){
    if(strcmp(topicElement,"led") == 0){
      if(led.getValue() == HIGH){
        client.publish(("unity/device/" + clientIDstr + "/value/led").c_str(),"1");
      }else {
        client.publish(("unity/device/" + clientIDstr + "/value/led").c_str(),"0");
      }
    }
    topicElement = strtok(NULL, "/");
  }
}

void action_event(char* topicElement, byte* payload){
  while(topicElement != NULL){
    Serial.println(topicElement);
    if(strcmp(topicElement, "led") == 0){
        topicElement = strtok(NULL, "/");
        if(topicElement != NULL){
          if(strcmp(topicElement,"heartbeat") == 0){
            Serial.println(topicElement);
            led.setHeartbeatInterval((int)payload[0]);
          }
        }else{
          if ((char)payload[0] == '1') {
            led.setValue(HIGH);
          }else{
            led.setValue(LOW);
          }
        }
    }
    topicElement = strtok(NULL, "/");
  }
}

void ping_event(byte* payload){
  Serial.print("Ping event received!");
  if ((char)payload[0] == '1') {
    Serial.print("Pinging back!");
    client.publish(("unity/device/" + clientIDstr + "/ping").c_str(), "1");
  }else{
    WiFi.disconnect(true);
  }
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Attempt to connect
    if (client.connect(clientID)) {
      Serial.println("connected");
      // Once connected, publish an announcement to unity: unity/connect/device-id
      client.publish(("unity/connect/"+clientIDstr+"/"+configID).c_str(), "1");
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
  led.update();
}
