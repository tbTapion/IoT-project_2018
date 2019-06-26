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

## License

Apache License, Version 2.0

## Copyright

Framework developed by Magnus Bärnholt and Andreas Lyngby
