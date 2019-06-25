#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <Wire.h>

// Update these with values suitable for your network.
//WiFi
const char *ssid = "MyExactNet";
const char *password = "MyExactNetPassword";
//MQTT
const char *mqtt_server = "192.168.4.1";
const int mqtt_port = 1883;
//WiFi + MQTT
const char *clientID; //Filled with mac address, unused, but kept in case of future functionality requiring it.
String clientIDstr;   //String containing mac address, used in conjunction with message building
String IP;            //String containing IP address.
String HOSTNAME;      //String containing HOSTNAME.
String configID = "redtile";
String deviceName = "toggle";

//Communication Objects
WiFiClient espClient;
PubSubClient client(espClient);

//Timer variables
unsigned long currentMillis;
unsigned long startMillis;
const unsigned long period = 30000;

void setup()
{
  Serial.begin(9600);
  Serial.println("Starting WiFi...");

  delay(1000);
  setup_wifi();
  client.setServer(mqtt_server, mqtt_port);
  client.setCallback(callback);

  startMillis = millis();
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
  //Connect to Unity again.
  client.publish(("unity/connect/" + clientIDstr + "/" + configID + "/" + deviceName).c_str(), "1");
}

void get_event(char *topicElement)
{
  while (topicElement != NULL)
  {
    if (strcmp(topicElement, "imu") == 0)
    {
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
    }
    else if (strcmp(topicElement, "toneplayer") == 0)
    {
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
  //Check RFID timer
  currentMillis = millis();
  if(currentMillis - startMillis >= period){
    String messageToSend = "0F0F0F0F0F0F0F0F";
    Serial.println("Sending message: " + messageToSend);
    client.publish(("unity/device/" + clientIDstr + "/event/rfid/read").c_str(), messageToSend.c_str());
    startMillis = currentMillis;
  }
}
