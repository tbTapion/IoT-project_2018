#include <BearSSLHelpers.h>
#include <CertStoreBearSSL.h>
#include <ESP8266WiFi.h>
#include <ESP8266WiFiAP.h>
#include <ESP8266WiFiGeneric.h>
#include <ESP8266WiFiMulti.h>
#include <ESP8266WiFiScan.h>
#include <ESP8266WiFiSTA.h>
#include <ESP8266WiFiType.h>
#include <WiFiClient.h>
#include <WiFiClientSecure.h>
#include <WiFiClientSecureAxTLS.h>
#include <WiFiClientSecureBearSSL.h>
#include <WiFiServer.h>
#include <WiFiServerSecure.h>
#include <WiFiServerSecureAxTLS.h>
#include <WiFiServerSecureBearSSL.h>
#include <WiFiUdp.h>

#include <Adafruit_ESP8266.h>

/*
Arduino ESP8266 WiFi and MQTT
*/

#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include "Led.h"
#include "PLab_PushButton.h"
#include <Wire.h>
#include "Adafruit_VL6180X.h"
#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
#include <avr/power.h>
#endif
#define PIN            2
#define PIN13          13
#define NUMPIXELS      12

// Update these with values suitable for your network.
//WiFi
const char* ssid = "pisbizarreadventure";
const char* password = "piberryrasp";
//MQTT
const char* mqtt_server = "192.168.42.1";
const int mqtt_port = 1883;
//Buzzer
const int speakerOut = 13; // Put speaker through 220 ohm on pin 13.
const int frequency = 200; // A6
const int tonePlay = 80; // half a second tone

//Time Of FLight
Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);
int delayval = 100; // delay for half a second
Adafruit_VL6180X vl = Adafruit_VL6180X();
//uint8_t range = vl.readRange();
//uint8_t status = vl.readRangeStatus();


WiFiClient espClient;
PubSubClient client(espClient);

Led led(BUILTIN_LED); //Built-in led as output
PLab_PushButton button(4); //Button class with pin nr. 4 passed

const char* clientID; //Filled with mac address, unused, but kept in case of future functionality requiring it. 
String clientIDstr; //String containing mac address, used in conjunction with message building

String configID = "cube3";

uint8_t previousRange;

void setup() {
  Serial.begin(115200);
  setup_wifi();
  client.setServer(mqtt_server, mqtt_port);
  client.setCallback(callback);

  //Buzzer setup--------------------
  pinMode(speakerOut, OUTPUT);

  //NoePixel Setup-----------------------------------
  pixels.begin(); // This initializes the NeoPixel library.

  //Time Of Flight Setup------------------------------
  Serial.begin(115200);

  // wait for serial port to open on native usb devices
  while (!Serial) {
    delay(1);
  }
  
  Serial.println("Adafruit VL6180x test!");
  if (! vl.begin()) {
    Serial.println("Failed to find sensor");
    while (1);
  }
  Serial.println("Sensor found!");
  
}//End of setup

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
      action_event(topicElement, payload);
    }else if(strcmp(topicElement,"get") == 0){
      get_event(topicElement);
    }/*else if(strcmp(topicElement,"getconfig") == 0){
      getconfig_event();
    }*/
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
    }else if(strcmp(topicElement, "button") == 0){
      if(button.isDown()){
        client.publish(("unity/device/" + clientIDstr + "/value/button").c_str(),"1");
      }else {
        client.publish(("unity/device/" + clientIDstr + "/value/button").c_str(),"0");
      }
    }
    topicElement = strtok(NULL, "/");
  }
}

void action_event(char* topicElement, byte* payload){
  while(topicElement != NULL){
    if(strcmp(topicElement, "led") == 0){
        if ((char)payload[0] == '1') {
          led.setValue(HIGH);
          lightsOn();
        }else{
          led.setValue(LOW);
          lightsOff();
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
  lightsConnected(50);
}

void loop() {
  delay(1);
  uint8_t range = vl.readRange();
  uint8_t status = vl.readRangeStatus();
  if (!client.connected()) {
    reconnect();
  }
  client.loop();
  //button.update();
  if (status == VL6180X_ERROR_NONE) {
    Serial.print("Range: "); Serial.println(range);
    if (range < 50 && previousRange >= 50) {
      client.publish(("unity/device/" + clientIDstr + "/event/button").c_str(), "1");
      led.setValue(LOW);
      lightsOff();
    }else if((range >= 50 || range == NULL ) && previousRange < 50){
      client.publish(("unity/device/" + clientIDstr + "/event/button").c_str(), "0");
    }
    previousRange = range;
  }
  
  //if(button.pressed()){
  // client.publish(("unity/device/" + clientIDstr + "/event/button").c_str(), "1");
  //}else if(button.released()){
  // client.publish(("unity/device/" + clientIDstr + "/event/button").c_str(), "0");
  //}
    
}

void lightsOn() {
  for(int i=0;i<NUMPIXELS;i++){
      // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
      pixels.setPixelColor(i, pixels.Color(0,50,25)); // Moderately bright green color.
      pixels.show(); // This sends the updated pixel color to the hardware.
   }
   tone(PIN13, frequency, 80);
}//End of lightsOn

void lightsOff(){
  for(int i=0;i<NUMPIXELS;i++){
      // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
      pixels.setPixelColor(i, pixels.Color(0,0,0)); // Moderately bright green color.
      pixels.show(); // This sends the updated pixel color to the hardware.
   }
   tone(PIN13, frequency, 80);
}

void lightsConnected(int delayval) {
  for(int i=0;i<NUMPIXELS;i++){
      // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
      pixels.setPixelColor(i, pixels.Color(0,50,25)); // Moderately bright green color.
      pixels.show(); // This sends the updated pixel color to the hardware.
      tone(PIN13, frequency, 20);
      delay(delayval); // Delay for a period of time (in milliseconds).
      }
   for(int i=0;i<NUMPIXELS;i++){
      // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
      pixels.setPixelColor(i, pixels.Color(0,0,0)); // Moderately bright green color.
      pixels.show(); // This sends the updated pixel color to the hardware.
      tone(PIN13, frequency, 20);
      delay(delayval); // Delay for a period of time (in milliseconds).
   }
}//End of lightsOn
