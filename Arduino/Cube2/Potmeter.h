#ifndef Potmeter_h
#define Potmeter_h

#include "Arduino.h"

class Potmeter {
    private:
        int _pin;
        int _value;
        int _previousValue;
        long _lasttime;
        int _timewait;
    public:
        Potmeter(int pin);
        char* getValue();
        bool checkWait();
};

#endif
