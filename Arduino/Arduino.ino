/*
Arduino ESP8266 WiFi and MQTT
*/

#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include "Led.h"
#include "Button.h"

// Update these with values suitable for your network.

const char* ssid = "pisbizarreadventure";
const char* password = "piberryrasp";
const char* mqtt_server = "192.168.42.1";
const int mqtt_port = 1883;
const char* mqtt_topic = "esp/led";

WiFiClient espClient;
PubSubClient client(espClient);
long lastMsg = 0;
char msg[50];
int value = 0;

Led led(BUILTIN_LED); //Built-in led as output
int buttonPrev = 0;
int pinButton = 4;

const char* clientID; //Filled with mac address

void setup() {
  pinMode(pinButton, INPUT);
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
  clientID = WiFi.macAddress().c_str();
  Serial.println(clientID);
}

void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");
  for (int i = 0; i < length; i++) {
    Serial.print((char)payload[i]);
  }
  Serial.println();

  // Switch on the LED if an 1 was received as first character
  if ((char)payload[0] == '1') {
    led.toggle();
  }
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Attempt to connect
    if (client.connect("ESP8266Client")) {
      Serial.println("connected");
      // Once connected, publish an announcement...
      client.publish("status/client-id/new", clientID);
      // ... and resubscribe
      client.subscribe("esp/#");
      client.subscribe("status/client-id/");
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
  if(digitalRead(pinButton) == HIGH){
    if(buttonPrev == 0){
      Serial.println("Button pressed!");
      Serial.print("Publish message: ");
      Serial.println("Button pressed!");
      client.publish("esp/button", "1");
      buttonPrev = 1;
    }
  }else {
    if(buttonPrev == 1){
      Serial.println("Button lifted!");
      Serial.print("Publish message: ");
      Serial.println("Button lifted!");
      client.publish("esp/button", "0");
      buttonPrev = 0;
    }
  }
  long now = millis();
  if (now - lastMsg > 30000) {
    lastMsg = now;
    Serial.print("Publish message: ");
    Serial.println("ping");
    client.publish("", "1");
  }
}
