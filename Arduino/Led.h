#ifndef Led_h
#define Led_h

#include "Arduino.h"

class Led {
    private:
        int _pin;
        int _value;
    public:
        Led(int pin);
        void toggle();
        void setValue(int value);
        int getValue();
};

#endif
