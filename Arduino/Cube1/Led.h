#ifndef Led_h
#define Led_h

#include "Arduino.h"

class Led {
    private:
        int _pin;
        int _value;
        long _lasttime;
        int _interval;
        bool _heartbeat;
    public:
        Led(int pin);
        void toggle();
        void setValue(int value);
        void setHeartbeatInterval(int interval);
        void heartbeat();
        int getValue();
        void update();
        bool checkWait(int interval);
};

#endif
