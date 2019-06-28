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

## License

Apache License, Version 2.0

## Copyright

Framework developed by Magnus Bärnholt and Andreas Lyngby
