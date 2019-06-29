#ifndef MQTTSocket_h
#define MQTTSocket_h

#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>

#if defined(ESP8266) || defined(ESP32)
#include <functional>
#define GET_CALLBACK_SIGNATURE std::function<void(char*, char*)> getEvent
#define ACTION_CALLBACK_SIGNATURE std::function<void(char*, char*, uint8_t*)> actionEvent
#define UNITYCONNECT_CALLBACK_SIGNATURE std::function<void()> helloEvent
#else
#define GET_CALLBACK_SIGNATURE void (*getEvent)(char*, char*)
#define ACTION_CALLBACK_SIGNATURE void (*actionEvent)(char*, char*, uint8_t*)
#define UNITYCONNECT_CALLBACK_SIGNATURE void (*helloEvent)()
#endif

class MQTTSocket {

private:
  String _ssid;
  String _ip;
  String _hostname;
  String _configID;
  String _deviceName;
  String _clientID;
  String _mqttBrokerIP;
  int _port;
  WiFiClient _espClient;
  PubSubClient _client;
  GET_CALLBACK_SIGNATURE;
  ACTION_CALLBACK_SIGNATURE;
  UNITYCONNECT_CALLBACK_SIGNATURE;
  void unityConnectEvent();
  void pingEvent();
  void pingEvent(byte *payload);

public:
  MQTTSocket();
  MQTTSocket(char *ssid, char *password);
  MQTTSocket(char *ssid, char *password, char *mqttBrokerIP);
  MQTTSocket(char *ssid, char *password, char *mqttBrokerIP, int port);
  void initConfiguration(const char *configID, const char *deviceName);
  void initWiFi(const char *ssid, const char *password);
  void initMQTT(const char *mqttBrokerIP);
  void initMQTT(const char *mqttBrokerIP, int port);
  void setCallbackGet(GET_CALLBACK_SIGNATURE);
  void setCallbackAction(ACTION_CALLBACK_SIGNATURE);
  void setCallbackHello(UNITYCONNECT_CALLBACK_SIGNATURE);
  void sendEvent(char *component, char *eventType, char *payload);
  void sendValue(char *component, char *eventType, char *payload);
  void myCallback(char *topic, unsigned char *payload, unsigned int length);
  void loop();
};

#endif
