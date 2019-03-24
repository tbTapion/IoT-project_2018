#ifndef RingLight_h
#define RingLight_h

#include <Adafruit_NeoPixel.h>

class RingLight {
  public:
    RingLight(int, int);
    void toggle();
    void setState(bool);
    void setColor(const byte*);
    void setIndivicualColor(const byte*, int);
    void setNumberOfActiveLeds(int);
    bool getState();
    byte* getColor();
    byte* getAllColors();
    byte* getIndividualColor(int);
  private:
    int _pin;
    int _activeLeds;
    int _numberOfLeds;
    bool _state;
    byte* _color;
    byte* _colors;
    Adafruit_NeoPixel _ringLight;
    void lightsOn();
    void lightsOff();
    void updateLights();
};

#endif
