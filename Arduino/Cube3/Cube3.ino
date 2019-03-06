/*
Arduino ESP8266 WiFi and MQTT
*/

#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <Wire.h>
#include "Adafruit_VL6180X.h"
#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
#include <avr/power.h>
#endif
#define PIN 2
#define PIN13 13
#define NUMPIXELS 24

// Update these with values suitable for your network.
//WiFi
const char *ssid = "IoT-WiFi";
const char *password = "40219917";
//MQTT
const char *mqtt_server = "10.42.0.1";
const int mqtt_port = 1883;
//Buzzer
const int speakerOut = 13; // Put speaker through 220 ohm on pin 13.
const int frequency = 200; // A6
const int tonePlay = 80;   // half a second tone
//Time Of FLight
Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);
Adafruit_VL6180X vl = Adafruit_VL6180X();

WiFiClient espClient;
PubSubClient client(espClient);

const char *clientID; //Filled with mac address, unused, but kept in case of future functionality requiring it.
String clientIDstr;   //String containing mac address, used in conjunction with message building
String IP;            //String containing IP address.
String HOSTNAME;      //String containing HOSTNAME.

String configID = "cube3";

uint8_t previousRange;

void setup()
{
  Serial.begin(115200);
  Serial.println("Starting WiFi...");
  delay(1000);
  setup_wifi();
  client.setServer(mqtt_server, mqtt_port);
  client.setCallback(callback);

  //Buzzer setup--------------------
  pinMode(speakerOut, OUTPUT);

  //NoePixel Setup-----------------------------------
  pixels.begin(); // This initializes the NeoPixel library.
  Serial.println("Loading NeoPixel... Success!");
  delay(500); // a second delay.

  //Time Of Flight Setup------------------------------
  Serial.begin(115200);

  // wait for serial port to open on native usb devices
  while (!Serial)
  {
    delay(500);
  }

  if (!vl.begin())
  {
    Serial.println("Failed to find VL6180X sensor");
    while (1)
      ;
  }
  Serial.println("Loading VL6180X sensor... Success!");
} //End of setup

void setup_wifi()
{
  delay(500);
  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED)
  {
    delay(100);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
  IP = WiFi.localIP().toString();
  Serial.println("IP address: " + IP);
  clientIDstr = WiFi.macAddress();
  clientID = clientIDstr.c_str();
  Serial.println("MAC address: " + clientIDstr);
  HOSTNAME = WiFi.hostname();
  Serial.println("Hostname: " + HOSTNAME);
}

void callback(char *topic, byte *payload, unsigned int length)
{
  Serial.print("New message arrived [");
  Serial.print(topic);
  Serial.print("] ");

  char *topicElement;
  topicElement = strtok(topic, "/");

  while (topicElement != NULL)
  {
    Serial.println(topicElement);
    if (strcmp(topicElement, "ping") == 0)
    {
      //Serial.println("Ping event space entered!");
      ping_event(payload);
    }
    else if (strcmp(topicElement, "action") == 0)
    {
      action_event(topicElement, payload);
    }
    else if (strcmp(topicElement, "get") == 0)
    {
      get_event(topicElement);
    }
    topicElement = strtok(NULL, "/");
  }
}

void get_event(char *topicElement)
{
  while (topicElement != NULL)
  {
    topicElement = strtok(NULL, "/");
  }
}

void action_event(char *topicElement, byte *payload)
{
  while (topicElement != NULL)
  {
    if (strcmp(topicElement, "led") == 0)
    {
      topicElement = strtok(NULL, "/");
      if (strcmp(topicElement, "color") == 0)
      {
        char payload_char[sizeof(payload)];
        for (int i = 0; i < sizeof(payload); i++)
        {
          payload_char[i] = char(payload[i]);
        }
        char *payloadelement = strtok(payload_char, "-");
        int rgb[3];
        for (int i = 0; i < 3; i++)
        {
          int value = (payloadelement[0] * 100) + (payloadelement[1] * 10) + payloadelement[2];
          payloadelement = strtok(NULL, "/");
        }
      }
      else
      {
        if ((char)payload[0] == '1')
        {
          lightsOn();
        }
        else
        {
          lightsOff();
        }
      }
    }
    topicElement = strtok(NULL, "/");
  }
} //End of action_event

void ping_event(byte *payload)
{
  Serial.print("Ping event received & ");
  if ((char)payload[0] == '1')
  {
    client.publish(("unity/device/" + clientIDstr + "/ping").c_str(), "1");
    Serial.print("returned.\n");
  }
  else
  {
    Serial.print("disconnecting WiFi in 3 sec...");
    delay(3000);
    WiFi.disconnect(true);
  }
} //End of ping_event

void reconnect()
{
  // Loop until we're reconnected
  while (!client.connected())
  {
    Serial.print("Connecting to MQTT server... ");
    // Attempt to connect
    if (client.connect(clientID))
    {
      Serial.println("Connected!");
      // Once connected, publish an announcement to unity: unity/connect/device-id
      client.publish(("unity/connect/" + clientIDstr + "/" + configID).c_str(), "1");
      // Then Subcribe to everything client-id/#
      client.subscribe((clientIDstr + "/#").c_str());
    }
    else
    {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println("Retrying in 5 seconds...");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
  lightsConnected(50);
}

void loop()
{
  delay(100);
  uint8_t range = vl.readRange();
  uint8_t status = vl.readRangeStatus();
  if (!client.connected())
  {
    reconnect();
  }
  client.loop();
  //button.update();
  if (status == VL6180X_ERROR_NONE)
  {
    Serial.print("Range: ");
    Serial.println(range);

    if (range < 50 && previousRange >= 50)
    {
      client.publish(("unity/device/" + clientIDstr + "/event/button").c_str(), "1");
      lightsOff();
    }

    else if ((range >= 50 || range == VL6180X_ERROR_ECEFAIL) && previousRange < 50)
    {
      Serial.println("ECE failure");
      client.publish(("unity/device/" + clientIDstr + "/event/button").c_str(), "0");
    }

    previousRange = range;
  }
} //End of loop

void lightsOn()
{
  for (int i = 0; i < NUMPIXELS; i++)
  {
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    pixels.setPixelColor(i, pixels.Color(0, 50, 25)); // Moderately bright green color.
    pixels.show();                                    // This sends the updated pixel color to the hardware.
  }
  tone(PIN13, frequency, 80);
} //End of lightsOn

void lightsOff()
{
  for (int i = 0; i < NUMPIXELS; i++)
  {
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    pixels.setPixelColor(i, pixels.Color(0, 0, 0)); // Moderately bright green color.
    pixels.show();                                  // This sends the updated pixel color to the hardware.
  }
  tone(PIN13, frequency, 80);
} //End of lightsOff

void lightsConnected(int delayval)
{
  for (int i = 0; i < NUMPIXELS; i++)
  {
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    pixels.setPixelColor(i, pixels.Color(0, 50, 25)); // Moderately bright green color.
    pixels.show();                                    // This sends the updated pixel color to the hardware.
    tone(PIN13, frequency, 20);
    delay(delayval); // Delay for a period of time (in milliseconds).
  }
  for (int i = 0; i < NUMPIXELS; i++)
  {
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    pixels.setPixelColor(i, pixels.Color(0, 0, 0)); // Moderately bright green color.
    pixels.show();                                  // This sends the updated pixel color to the hardware.
    tone(PIN13, frequency, 20);
    delay(delayval); // Delay for a period of time (in milliseconds).
  }
} //End of lightsConnected
