#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <Wire.h>
#include "Adafruit_VL6180X.h"
#include <Adafruit_NeoPixel.h>
#include <MPU9250_RegisterMap.h>
#include <SparkFunMPU9250-DMP.h>
#include <imumaths.h>

#ifdef __AVR__
#include <avr/power.h>
#endif

#define NEOPIXELPIN 2
#define BUZZERPIN 13
#define NUMPIXELS 24

// Update these with values suitable for your network.
//WiFi
const char *ssid = "MyExactNet";
const char *password = "MyExactNetPassword";
//MQTT
const char *mqtt_server = "192.168.4.1";
const int mqtt_port = 1883;
//Buzzer
const int tonePlay = 80; // half a second tone
//WiFi + MQTT
const char *clientID; //Filled with mac address, unused, but kept in case of future functionality requiring it.
String clientIDstr;   //String containing mac address, used in conjunction with message building
String IP;            //String containing IP address.
String HOSTNAME;      //String containing HOSTNAME.
String configID = "redtile";
String deviceName = "toggle";
//Neopixel vars
byte individualLedColors[NUMPIXELS * 3];
int numberOfActiveLeds = 12;

//Rotation vars
byte rotation[6];

//Communication Objects
WiFiClient espClient;
PubSubClient client(espClient);

//Arduino components
MPU9250_DMP imu;
Adafruit_NeoPixel ringLight = Adafruit_NeoPixel(NUMPIXELS, NEOPIXELPIN, NEO_GRB + NEO_KHZ800);
//Adafruit_VL6180X sensor = Adafruit_VL6180X();

void setup()
{
  Serial.begin(9600);
  Serial.println("Starting WiFi...");

  delay(1000);
  setup_wifi();
  client.setServer(mqtt_server, mqtt_port);
  client.setCallback(callback);

  //Buzzer setup--------------------
  pinMode(BUZZERPIN, OUTPUT);

  //NoePixel Setup-----------------------------------
  ringLight.begin(); // This initializes the NeoPixel library.
  Serial.println("Loading NeoPixel... Success!");
  delay(500); // a second delay.

  /*if (!sensor.begin())
  {
    Serial.println("Failed to find sensor.");
    while (1)
      ;
  }
  Serial.println("Sensor found!");*/

  // Call imu.begin() to verify communication and initialize
  while (imu.begin() != INV_SUCCESS)
  {
    Serial.println("Unable to communicate with MPU-9250");
    Serial.println("Check connections, and try again.");
    Serial.println();
    delay(2000);
  }
  int success = imu.dmpBegin(DMP_FEATURE_6X_LP_QUAT |                    // Enable 6-axis quat
                                 DMP_FEATURE_GYRO_CAL | DMP_FEATURE_TAP, // Use gyro calibration
                             10);                                        // Set DMP FIFO rate to 10 Hz
  // DMP_FEATURE_LP_QUAT can also be used. It uses the
  // accelerometer in low-power mode to estimate quat's.
  // DMP_FEATURE_LP_QUAT and 6X_LP_QUAT are mutually exclusive

  // dmpSetTap parameters, in order, are:
  // x threshold: 1-1600 (0 to disable)
  // y threshold: 1-1600 (0 to disable)
  // z threshold: 1-1600 (0 to disable)
  // (Threshold units are mg/ms)
  // taps: Minimum number of taps needed for interrupt (1-4)
  // tap time: milliseconds between valid taps
  // tap time multi: max milliseconds between multi-taps
  unsigned short xThresh = 0;     // Disable x-axis tap
  unsigned short yThresh = 0;     // Disable y-axis tap
  unsigned short zThresh = 20;    // Set z-axis tap thresh to 100 mg/ms
  unsigned char taps = 1;         // Set minimum taps to 1
  unsigned short tapTime = 300;   // Set tap time to 100ms
  unsigned short tapMulti = 1000; // Set multi-tap time to 1s
  imu.dmpSetTap(xThresh, yThresh, zThresh, taps, tapTime, tapMulti);

  Serial.print(success);
  //Setting default color for ringlight leds
  for (int i = 0; i < NUMPIXELS; i++)
  {
    individualLedColors[(i * 3)] = 0;
    individualLedColors[(i * 3) + 1] = 150;
    individualLedColors[(i * 3) + 2] = 0;
  }
  lightsConnected(20);

  for(int i = 0; i < 6; i++){
    rotation[i] = 0;
  }
}

void setup_wifi()
{
  delay(500);
  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.mode(WIFI_STA);
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

int payloadIndex = 0;
bool ringLightState = false;

void callback(char *topic, byte *payload, unsigned int length)
{
  Serial.print("New message arrived [");
  Serial.print(topic);
  Serial.println("] ");

  payloadIndex = 0;

  char *topicElement;
  topicElement = strtok(topic, "/");

  while (topicElement != NULL)
  {
    Serial.println(topicElement);
    if (strcmp(topicElement, "ping") == 0)
    {
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
    else if (strcmp(topicElement, "hello") == 0)
    {
      hello_event();
    }
    topicElement = strtok(NULL, "/");
  }
}

void hello_event()
{
  //Reset all output components(in this case the ring light)
  lightsOff();
  for (int i = 0; i < NUMPIXELS; i++)
  {
    individualLedColors[(i * 3)] = 0;
    individualLedColors[(i * 3) + 1] = 150;
    individualLedColors[(i * 3) + 2] = 0;
  }
  //Connect to Unity again.
  client.publish(("unity/connect/" + clientIDstr + "/" + configID + "/" + deviceName).c_str(), "1");
}

void get_event(char *topicElement)
{
  while (topicElement != NULL)
  {
    if (strcmp(topicElement, "imu") == 0)
    {
      topicElement = strtok(NULL, "/");
      if (strcmp(topicElement, "rotation") == 0)
      {
        char payload[sizeof(rotation) + 1];
        for (int i = 0; i < 6; i++)
        {
          payload[i] = (char)rotation[i];
        }
        payload[sizeof(rotation)] = '\0';
        client.publish(("unity/device/" + clientIDstr + "/value/imu/rotation").c_str(), payload);
      }
    }
    topicElement = strtok(NULL, "/");
  }
}

void action_event(char *topicElement, byte *payload)
{
  while (topicElement != NULL)
  {
    if (strcmp(topicElement, "ringlight") == 0)
    {
      topicElement = strtok(NULL, "/");
      if (strcmp(topicElement, "color") == 0)
      {
        for (int i = 0; i < NUMPIXELS; i++)
        {
          individualLedColors[(i * 3)] = payload[(payloadIndex + 1) + 0];
          individualLedColors[(i * 3) + 1] = payload[(payloadIndex + 1) + 1];
          individualLedColors[(i * 3) + 2] = payload[(payloadIndex + 1) + 2];
        }
        lightsOn();
        payloadIndex = (payloadIndex + 4);
      }
      else if (strcmp(topicElement, "state") == 0)
      {
        if (payload[payloadIndex + 1] == 1)
        {
          lightsOn();
        }
        else if(payload[payloadIndex + 1] == 0)
        {
          lightsOff();
        }
        payloadIndex = (payloadIndex + 2);
      }
      else if (strcmp(topicElement, "all_colors") == 0)
      {
        for (int i = 0; i < NUMPIXELS; i++)
        {
          individualLedColors[(i * 3)] = payload[(payloadIndex + 1) + (i * 3)];
          individualLedColors[(i * 3) + 1] = payload[(payloadIndex + 1) + (i * 3) + 1];
          individualLedColors[(i * 3) + 2] = payload[(payloadIndex + 1) + (i * 3) + 2];
        }
        lightsOn();
        payloadIndex = payloadIndex + (NUMPIXELS * 3) + 1;
      }
      else if (strcmp(topicElement, "number_of_leds") == 0)
      {
        numberOfActiveLeds = payload[payloadIndex + 1];
        lightsOn();
        payloadIndex = payloadIndex + 2;
      }
    }
    else if (strcmp(topicElement, "toneplayer") == 0)
    {
      topicElement = strtok(NULL, "/");
      if (strcmp(topicElement, "play") == 0)
      {
        int frequency = 0;
        for (int i = 0; i < 4; i++)
        {
          frequency = frequency + payload[payloadIndex + 1 + i] * pow(256, i);
        }
        tone(BUZZERPIN, frequency);
        payloadIndex = payloadIndex + 5;
      }
      else if (strcmp(topicElement, "stop") == 0)
      {
        noTone(BUZZERPIN);
        payloadIndex = payloadIndex + 2;
      }
      else if (strcmp(topicElement, "frequency_duration") == 0)
      {
        int frequency = 0;
        int duration = 0;
        for (int i = 0; i < 4; i++)
        {
          frequency = frequency + payload[payloadIndex + 1 + i] * pow(256, i);
          duration = duration + payload[payloadIndex + 5 + i] * pow(256, i);
        }
        tone(BUZZERPIN, frequency, duration);
        payloadIndex = payloadIndex + 9;
      }
    }
    topicElement = strtok(NULL, "/");
  }
} //End of action_event

void ping_event(byte *payload)
{
  Serial.print("Ping event received & ");
  if (payload[0] == 1)
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
      client.publish(("unity/connect/" + clientIDstr + "/" + configID + "/" + deviceName).c_str(), "1");
      // Then Subcribe to everything client-id/#
      client.subscribe((clientIDstr + "/#").c_str());
      client.subscribe("esp8266/#");
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
}

void loop()
{
  if (!client.connected())
  {
    reconnect();
  }
  client.loop();
  // Check for new data in the FIFO
  if (imu.fifoAvailable())
  {
    // Serial.println("loop2");
    // Use dmpUpdateFifo to update the ax, gx, mx, etc. values
    if (imu.dmpUpdateFifo() == INV_SUCCESS)
    {
      // computeEulerAngles can be used -- after updating the
      // quaternion values -- to estimate roll, pitch, and yaw
      imu.computeEulerAngles();
      printIMUData();
      if (imu.tapAvailable())
      {
        unsigned char tapDir = imu.getTapDir();
        unsigned char tapCnt = imu.getTapCount();
        if ((tapDir = TAP_Z_UP) || (tapDir = TAP_Z_DOWN))
        {
          client.publish(("unity/device/" + clientIDstr + "/event/imu/tapped").c_str(), "1");
          Serial.print("Tap Z");
        }
      }
    }
  }
}

bool first_reading = true;
float first_pitch = 0.0;
float first_roll = 0.0;

void printIMUData(void)
{
  // After calling dmpUpdateFifo() the ax, gx, mx, etc. values
  // are all updated.
  // Quaternion values are, by default, stored in Q30 long
  // format. calcQuat turns them into a float between -1 and 1
  float q0 = imu.calcQuat(imu.qw);
  float q1 = imu.calcQuat(imu.qx);
  float q2 = imu.calcQuat(imu.qy);
  float q3 = imu.calcQuat(imu.qz);

  Quaternion q(q0, q1, q2, q3);
  Vector<3> qEuler = q.toEuler();
  Vector<3> qEulerDeg(qEuler.x() * 180.0 / M_PI, qEuler.y() * 180.0 / M_PI, qEuler.z() * 180.0 / M_PI);

  float pitchAngle = -qEulerDeg.y();
  float rollAngle = -qEulerDeg.z();
  float yawAngle = qEulerDeg.x();

  /*int roll = (rollAngle + 1) * 180;
  int pitch = (pitchAngle + 1) * 180;
  int yaw = (yawAngle + 1) * 180;
  */
  rotation[0] = (int)rollAngle + 180 & 0xFF;
  rotation[1] = ((int)rollAngle + 180 >> 8) & 0xFF;
  rotation[2] = (int)pitchAngle + 180 & 0xFF;
  rotation[3] = ((int)pitchAngle + 180 >> 8) & 0xFF;
  rotation[4] = (int)yawAngle + 180 & 0xFF;
  rotation[5] = ((int)yawAngle + 180 >> 8) & 0xFF;

  /*
   if (first_reading) {
    first_reading = false;
    first_pitch = pitchAngle;
    first_roll = rollAngle;
   } else {
    pitchAngle = pitchAngle - first_pitch;
    rollAngle = rollAngle - first_roll;
   }
*/

  // Serial.println(" R/P/Y: " + String(rollAngle) + ", "
  //           + String(pitchAngle) + ", " + String(yawAngle));
}

// Set the first noOfPixelsOn pixels on ringLightBar to color.
// Mustbe called pixelBar(&ringLight, n, ringLight.Color(r, g, b));
// DS 2018...
void pixelBar(Adafruit_NeoPixel *ringLightBar, int noOfPixelsOn, uint32_t color)
{
  int noOfPixelsTotal = ringLightBar->numPixels();
  // Serial.print("rgb: "); Serial.print(r); Serial.print(",");Serial.print(g); Serial.print(","); Serial.println(b);
  if ((noOfPixelsOn >= 0) && (noOfPixelsOn <= noOfPixelsTotal))
  {
    for (uint16_t i = 0; i < noOfPixelsOn; i++)
    {
      ringLightBar->setPixelColor(i, color);
      ringLightBar->show();
    }
    for (uint16_t i = noOfPixelsOn; i < noOfPixelsTotal; i++)
    {
      ringLightBar->setPixelColor(i, 0);
      ringLightBar->show();
    }
  }
}

void lightsOn()
{
  for (int i = 0; i < numberOfActiveLeds; i++)
  {
    int red = individualLedColors[(i * 3)];
    int green = individualLedColors[(i * 3) + 1];
    int blue = individualLedColors[(i * 3) + 2];
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    ringLight.setPixelColor(i, ringLight.Color(red, green, blue)); // Moderately bright green color.
    ringLight.show();                                              // This sends the updated pixel color to the hardware.
  }
  for(int i = numberOfActiveLeds; i < NUMPIXELS; i++){
    ringLight.setPixelColor(i, ringLight.Color(0, 0, 0)); // Moderately bright green color.
    ringLight.show();                                     // This sends the updated pixel color to the hardware.
  }
} //End of lightsOn

void lightsOff()
{
  for (int i = 0; i < NUMPIXELS; i++)
  {
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    ringLight.setPixelColor(i, ringLight.Color(0, 0, 0)); // Moderately bright green color.
    ringLight.show();                                     // This sends the updated pixel color to the hardware.
  }
} //End of lightsOff

void lightsConnected(int delayval)
{
  for (int i = 0; i < NUMPIXELS; i++)
  {
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    ringLight.setPixelColor(i, ringLight.Color(0, 50, 25)); // Moderately bright green color.
    ringLight.show();                                       // This sends the updated pixel color to the hardware.
    //tone(BUZZERPIN, 200, 20);
    delay(delayval); // Delay for a period of time (in milliseconds).
  }
  for (int i = 0; i < NUMPIXELS; i++)
  {
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    ringLight.setPixelColor(i, ringLight.Color(0, 0, 0)); // Moderately bright green color.
    ringLight.show();                                     // This sends the updated pixel color to the hardware.
    //tone(BUZZERPIN, 200, 20);
    delay(delayval); // Delay for a period of time (in milliseconds).
  }
} //End of lightsConnected
