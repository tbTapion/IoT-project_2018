#include <Adafruit_NeoPixel.h>
#include "RingLight.h"

RingLight::RingLight(int pin, int numberOfPixels){
  _ringLight = Adafruit_NeoPixel(numberOfPixels, pin, NEO_GRB + NEO_KHZ800);
  _pin = pin;
  _activeLeds = numberOfPixels;
  _numberOfLeds = numberOfPixels;
  _state = false;
  _colors = new byte[numberOfPixels * 3];
  for(int i = 0; i<numberOfPixels;i++){
    _colors[i] = 255;
    _colors[i+1] = 0;
    _colors[i+2] = 0;
  }
}

void RingLight::toggle(){
  setState(!_state);
}

void RingLight::setState(bool state){
  _state = state;
  updateLights();
}

void RingLight::setColor(const byte* color){
  for(int i = 0; i<_numberOfLeds;i++){
    _colors[i] = color[0];
    _colors[i+1] = color[1];
    _colors[i+2] = color[2];
  }
  updateLights();
}

void RingLight::setIndivicualColor(const byte* color, int i){
  _colors[i*3] = color[0];
  _colors[i*3 + 1] = color[1];
  _colors[i*3 + 2] = color[2];
  updateLights();
}

void RingLight::setNumberOfActiveLeds(int numOfActive){
  _activeLeds = numOfActive;
}

bool RingLight::getState(){
  return _state;
}

byte* RingLight::getColors(){
  return _colors;
}

byte* getIndividualColor(int i){
  byte tempColor[3];
  tempColor[0] = _colors[i *3];
  tempColor[1] = _colors[i *3 + 1];
  tempColor[2] = _colors[i *3 + 2];
  return tempColor;
}

void RingLight::updateLights(){
  if(_state){
    lightsOn();
  }else{
    lightsOff();
  }
}

void RingLight::lightsOn()
{
  for (int i = 0; i < _activeLeds; i++)
  {
    int red = _colors[(i * 3)];
    int green = _colors[(i * 3) + 1];
    int blue = _colors[(i * 3) + 2];
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    _ringLight.setPixelColor(i, _ringLight.Color(red, green, blue)); // Moderately bright green color.
                                       // This sends the updated pixel color to the hardware.
  }
  _ringLight.show();
} //End of lightsOn

void RingLight::lightsOff()
{
  for (int i = 0; i < _numberOfLeds; i++)
  {
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    _ringLight.setPixelColor(i, ringLight.Color(0, 0, 0)); // Moderately bright green color.
    _ringLight.show();                                  // This sends the updated pixel color to the hardware.
  }
  _ringLight.show();
} //End of lightsOff
