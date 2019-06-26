#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include "MQTTSocket.h"

MQTTSocket::MQTTSocket(){
  _port = 1883;
  _client = NULL;
  _espClient = NULL;
}
//MQTTSocket(char *ssid, char *password);
//MQTTSocket(char *ssid, char *password, char *mqttBrokerIP);
//MQTTSocket(char *ssid, char *password, char *mqttBrokerIP, int port);
void MQTTSocket::initConfiguration(char *configID, char *deviceName){
  _configID = configID;
  _deviceName = deviceName;
}

void MQTTSocket::initMQTT(char *mqttBrokerIP){
  _mqttBrokerIP = mqttBrokerIP;

  PubSubClient client(_espClient);
  _client = client;
  _client.setServer(_mqttBrokerIP, _port);
  _client.setCallback(callback);
}

void MQTTSocket::initMQTT(char *mqttBrokerIP, int port){
  _mqttBrokerIP = mqttBrokerIP;
  _port = port;

  PubSubClient client(_espClient);
  _client = client;
  _client.setServer(_mqttBrokerIP, _port);
  _client.setCallback(callback);
}

void MQTTSocket::initWiFi(char *ssid, char *password){
  WiFiClient espClient;
  _espClient = espClient;
  Serial.println("Starting WiFi...");
  delay(500);
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.mode(WIFI_STA);
  while(WiFi.status() != WL_CONNECTED){
    delay(100);
    Serial.print(".");
  }

  Seral.println("");
  Serial.println("WiFi connected!");
  s_ip = WiFi.localIP().toString();
  _hostname = WiFi.hostname();
  _clientID = WiFi.macAddress();
}

void callback(char *topic, byte *payload, unsigned int length)
{
  Serial.print("New message arrived [");
  Serial.print(topic);
  Serial.println("] ");

  char *topicElement;
  char *idReceived = strtok(topic, "/");
  Serial.println("ID received: ");
  Serial.print(idReceived);
  char *messageType = strtok(NULL, "/");
  Serial.println("Message type: ");
  Serial.print(messageType);

  if(strcmp(messageType, "ping") == 0){
    pingEvent(payload);
  }else if(strcmp(messageType, "hello") == 0){
    unityConnectEvent();
  }

  char *component = strtok(NULL, "/");
  char *eventType = strtok(NULL, "/");
  while (component != NULL)
  {
    Serial.println(component);
    Serial.println(eventType);

    if (strcmp(topicElement, "action") == 0)
    {
      actionEvent(component, eventType, payload);
    }
    else if (strcmp(topicElement, "get") == 0)
    {
      getEvent(component, eventType);
    }

    component = strtok(NULL, "/");
    eventType = strtok(NULL, "/");
  }
}

void MQTTSocket::pingEvent(byte *payload)
{
  Serial.print("Ping event received & ");
  if (payload[0] == 1)
  {
    client.publish(("unity/device/" + _clientID + "/ping").c_str(), "1");
    Serial.print("returned.\n");
  }
  else
  {
    Serial.print("disconnecting WiFi in 3 sec...");
    delay(3000);
  }
    WiFi.disconnect(true);
}

void MQTTSocket::unityConnectEvent(){
  client.publish(("unity/connect/" + _clientID + "/" + _configID + "/" + _deviceName).c_str(), "1");
  helloEvent();
}

void MQTTSocket::setCallbackGet(GET_CALLBACK_SIGNATURE){
  this->getEvent = getEvent;
}

void MQTTSocket::setCallbackAction(ACTION_CALLBACK_SIGNATURE){
  this->actionEvent = actionEvent;
}

void MQTTSocket::sendEvent(string component, string eventType, byte *payload){
  _client.publish(("unity/device/" + _clientID + "/event/" + component + "/" + eventType).c_str(), payload);
}

void MQTTSocket::sendValue(char *component, char *eventType, byte *payload){
  _client.publish(("unity/device/" + _clientID + "/event/" + component + "/" + eventType).c_str(), payload);
}

void MQTTSocket::loop(){
  if(!_client.connected()){
    while(!_client.connected()){
      Serial.print("Connecting to MQTT server... ");
      if(_client.connect(_clientID)){
        Serial.println("Connected!");
        _client.publish(("unity/connect/" + _clientIDstr + "/" + _configID + "/" + _deviceName).c_str(), "1");
        _client.subscribe((_clientID + "/#").c_str());
        _client.subscribe("unityconnect/#");
      }else{
        Serial.print("failed, rc=");
        Serial.print(_client.state());
        Serial.println("Retrying in 5 seconds...");
        delay(5000);
      }
    }
  }
  _client.loop();
}
