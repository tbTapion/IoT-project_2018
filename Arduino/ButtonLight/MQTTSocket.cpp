#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include "MQTTSocket.h"

MQTTSocket::MQTTSocket(){
  _port = 1883;
}

MQTTSocket::MQTTSocket(char *ssid, char *password){
  _port = 1883;
  initWiFi(ssid, password);
}

MQTTSocket::MQTTSocket(char *ssid, char *password, char *mqttBrokerIP){
  _port = 1883;
  initWiFi(ssid, password);
  initMQTT(mqttBrokerIP);
}

MQTTSocket::MQTTSocket(char *ssid, char *password, char *mqttBrokerIP, int port){
  _port = port;
  initWiFi(ssid, password);
  initMQTT(mqttBrokerIP, port);
}

void MQTTSocket::initConfiguration(const char *configID, const char *deviceName){
  _configID = configID;
  _deviceName = deviceName;
}

void MQTTSocket::initMQTT(const char *mqttBrokerIP){
  initMQTT(mqttBrokerIP, 1883);
}

void MQTTSocket::initMQTT(const char *mqttBrokerIP, int port){
  _mqttBrokerIP = mqttBrokerIP;
  _port = port;

  PubSubClient client(_espClient);
  _client = client;
  _client.setServer(_mqttBrokerIP.c_str(), _port);
//  std::function<void(char*, unsigned char*, unsigned int)> callback = [=](char *a, unsigned char *b, unsigned int c) {this->myCallback(a, b, c);};
  _client.setCallback([=](char *topic, unsigned char *payload, unsigned int length) {this->myCallback(topic, payload, length);});
}

void MQTTSocket::initWiFi(const char *ssid, const char *password){
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

  Serial.println("");
  Serial.println("WiFi connected!");
  _ip = WiFi.localIP().toString();
  _hostname = WiFi.hostname();
  _clientID = WiFi.macAddress();
}

void MQTTSocket::myCallback(char *topic, unsigned char *payload, unsigned int length)
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

  int payloadIndex = 0;
  int nextComponentPayload = payload[payloadIndex] + 1;
  int payloadSize = payload[payloadIndex];

  if(strcmp(messageType, "ping") == 0){
    pingEvent(payload);
  }else if(strcmp(messageType, "hello") == 0){
    unityConnectEvent();
  }

  char *component = strtok(NULL, "/");
  char *eventType = strtok(NULL, "/");
  byte * payloadPart;
  while (component != NULL)
  {
    Serial.println(component);
    Serial.println(eventType);
    
    //extract the next part of the payload bundle and put it into payloadPart
    std::copy(payload + payloadIndex +1, payload + payloadIndex + payloadSize, payloadPart);

    if (strcmp(topicElement, "action") == 0)
    {
      actionEvent(component, eventType, payloadPart);
    }
    else if (strcmp(topicElement, "get") == 0)
    {
      getEvent(component, eventType);
    }

    payloadIndex = payloadIndex + payloadSize + 1 // 1 added to reach the next divider byte
    payloadSize = payload[payloadIndex] //Next payload size

    component = strtok(NULL, "/");
    eventType = strtok(NULL, "/");
  }
}

void MQTTSocket::pingEvent(byte *payload)
{
  Serial.print("Ping event received & ");
  if (payload[0] == 1)
  {
    _client.publish(("unity/device/" + _clientID + "/ping").c_str(), "1");
    Serial.print("returned.\n");
  }
  else
  {
    Serial.print("disconnecting WiFi in 3 sec...");
    delay(3000);
    WiFi.disconnect(true);
  }
}

void MQTTSocket::unityConnectEvent(){
  _client.publish(("unity/connect/" + _clientID + "/" + _configID + "/" + _deviceName).c_str(), "1");
  helloEvent();
}

void MQTTSocket::setCallbackGet(GET_CALLBACK_SIGNATURE){
  this->getEvent = getEvent;
}

void MQTTSocket::setCallbackAction(ACTION_CALLBACK_SIGNATURE){
  this->actionEvent = actionEvent;
}

void MQTTSocket::setCallbackHello(UNITYCONNECT_CALLBACK_SIGNATURE){
  this->helloEvent = helloEvent;
}

void MQTTSocket::sendEvent(char* component, char* eventType, char *payload){
  _client.publish(("unity/device/" + _clientID + "/event/" + component + "/" + eventType).c_str(), payload);
}

void MQTTSocket::sendValue(char *component, char *eventType, char *payload){
  _client.publish(("unity/device/" + _clientID + "/event/" + component + "/" + eventType).c_str(), payload);
}

void MQTTSocket::loop(){
  if(!_client.connected()){
    while(!_client.connected()){
      Serial.print("Connecting to MQTT server... ");
      if(_client.connect(_clientID.c_str())){
        Serial.println("Connected!");
        _client.publish(("unity/connect/" + _clientID + "/" + _configID + "/" + _deviceName).c_str(), "1");
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
