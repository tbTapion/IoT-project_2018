# IoT-project_2018

## MQTT Library Used in the Implementation

The MQTT library used and found in the MQTT folder under the Unity folder can be found originally here: https://github.com/vovacooper/Unity3d_MQTT
This MQTT library is based on Microsoft's C# MQTT library. https://code.msdn.microsoft.com/windowsdesktop/M2Mqtt-MQTT-client-library-ac6d3858

## Usage/installation

### Unity

Add/import the MQTT folder and the ExactFramework folder to your Unity project.

The MQTT folder has the MQTT library used in the framework.
The ExactFramework folder has the framework itself with accompanying examples.

If this doesn't work, try opening the project in the Unity-old folder and use that.

### Raspberry Pi

Follow the guide over at https://www.raspberrypi.org/documentation/configuration/wireless/access-point.md if you want to use a raspberry pi for the MQTT broker.
Follow this guide and then install the mosquitto broker.

```
sudo apt install mosquitto
```

If the package isn't found, try running `sudo apt update` first.

After the package is installed, restart the RPi and the mosquitto broker should run as a service in the background on port 1883.

### Arduino

The Arduino programs have been developed using Arduinos of type ESP8266, hence the WiFi and MQTT Libraries are suited those.
The MQTT Library used for the Arduino also supports other ESP models and Arduino models.

MQTT Library by Nick O'Leary, version 2.7.0: PubSubClient
Can be found in the Library Manager of the Arduino IDE, as well as here https://github.com/knolleary/pubsubclient

Boards and WiFi libraries by espressif: https://github.com/espressif

## Information

Also found on the wiki for the project. Navigate to it at the top.

### What is this project?

This project is the thesis work of Magnus Bärnholt and Andreas Lyngby for their thesis. The aim of this project was to make a framework in Unity for connecting devices using the MQTT messaging protocol to Unity, so they can be used in games, etc. 

### Why use MQTT and not some other message protocol?

MQTT is an ISO standard (https://www.iso.org/standard/69466.html) for messaging in internet of things. It's also very lightweight and suitable for devices like Arduinos. 

### Do I need your Arduino code to work with the project?

No. As said earlier, this project aims to connect any device using MQTT to Unity. If you can set up the correct message structure (topics in MQTT) that fits with the framework, then any device using MQTT should work with the framework. You may need to develop subclasses for the specific device and its components, but we do have multiple examples of this.

### What part of the code is the framework, and what's extra code?

The main part of the framework are the three classes MQTTHandler.cs, TwinObject.cs and DeviceComponent.cs. All the other classes bundled with the framework are either extra helping classes or examples for how to use the framework. Each configuration in the "ExampleConfigurations" folder is an example of a configuration setup where the components(examples in the folder "ExampleComponents") are all developed towards some Arduino prototypes using the components specified. Eh. a "tile" object using a Ring Light, Time of Flight sensor, Tone Player, and an IMU. 

### Old information?

The wiki might have some old documentation about the classes either bundled with the framework, or in the framework itself. 

## MQTT Message structure

MQTT message consists of two parts. A topic and a payload. The topics are sort of a message identification that can be formed however you want. A topic can consist of multiple parts separated by the delimiter "/". Some examples are "unity/deviceID/connect" and "unity/deviceID/event". Here we have two separate message topics that could be subscribed to separately to only receieve messages within those topics. With the delimiter "/", a client could subscribe to just the "unity/deviceID" part of the topics to receive messages from both of the other two topics. To do this, the client would have to suscribe to the topic "unity/deviceID/#". The '#' part of the topic means that the client is subscribing to all sub-topics within that topic. This means it receives messages from both the "connect" and "event topics as well. 

With this, we have two general topic types in our system; the "unity/#" topic that the Unity game subscribes to, and the "deviceID/#" that the Arduinos or other clients subscribe to. "deviceID" here being the unique ID of each device connecting, in the Arduinos case their MAC-address. We also have a special "unityconnect/hello" message that's sent whenever unity goes online. This gives us the following message types.

### Action Message

These are messages sent from unity to a device to update one of its actuators. The general form of this message is "deviceID/action/component/updatetype". Here the deviceID is the MAC-address of the device for the particular setups we have, action is the message type, component is the component/actuator to update, and updatetype is what type of update we want to do. An updatetype could be the state of something, color of a light, etc. 

### Event Message

These are messages sent from a client to unity to update its digital presentation within unity. If a button on the client is pressed, an event message is sent to unity to update its state. Same with a release. The general form of this message is "unity/deviceID/event/component/updatetype". Here the deviceID is the MAC-address of the device for the particular setups we have, event is the message type, component is the component/sensor to update, and updatetype is what type of update we want to do. An updatetype could be the state of something, sensor data, etc. 

### Get Message

These are messages sent from unity to a device to get the value of typically a sensor. This could be a replacement of a continuous message stream from the device client to unity. The general form of this message is "deviceID/get/component/valuetype". Here the deviceID is the MAC-address of the device for the particular setups we have, get is the message type, component is the component/sensor to get a value from, and valuetype is what type of value we want to do. A valuetype could be the state of something, other sensor data, etc.

### Value Message

These are messages sent from a client to unity as a response to a get message received. The value message is usually to sent the requested value back to unity. The general form of this message is "unity/deviceID/value/component/valuetype". Here the deviceID is the MAC-address of the device for the particular setups we have, value is the message type, component is the component/sensor to get a value from, and valuetype is what type of value we want to do. A valuetype could be the state of something, other sensor data, etc. In actuality, this message type is very similar to the event message type and could be handled by the same methods in Unity, but have different methods in case we want different handling.

### Ping message

The ping message type is a simple message initiated from Unity in the form of "deviceID/ping" to see if the device is still connected to the system. As a response to this, the device sends a "unity/deviceID/ping" message letting Unity know the device is still connected. If Unity doesn't receive a response from the device, it unlinks that digital twin so another device can connect to it. 

### Unity Connect / Hello Message

This message is one that Unity sends out when it connects to the MQTT broker. It's in the form of "unityconnect/hello". In our setup, when a device receives this message, it resets its output values to start out fresh and sends a new connect message to unity. 

## License

Apache License, Version 2.0

## Copyright

Framework developed by Magnus Bärnholt and Andreas Lyngby
