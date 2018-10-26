/*
Arduino button, takes pin number and MQTT client.
*/

#ifndef Button_h
#define Button_h

#include "Arduino.h"

class Button {
    private:
        int _pin;
        byte _pressed, _checked;
        byte _currentState, _previousState;
        long _lastTime;
    public:
        Button(int pin);
        void update();
        boolean pressed();
        boolean lifted();
};

#endif
