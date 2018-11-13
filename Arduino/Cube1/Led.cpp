#include "Arduino.h"
#include "Led.h"

Led::Led(int pin){
    _pin = pin;
    _value = HIGH;
    _lasttime = millis();
    pinMode(_pin, OUTPUT);
    digitalWrite(_pin, _value);
}

void Led::toggle(){
    _value = !_value;
    digitalWrite(_pin, _value);
}

void Led::setValue(int value){
    _value = value;
    digitalWrite(_pin, _value);
}

int Led::getValue(){
    return _value;
}

void Led::setHeartbeatInterval(int interval){
    _interval = interval;
    if(_interval > 0){
        _heartbeat = true;
    }else{
        _heartbeat = false;
        setValue(_value);
    }
}

void Led::update(){
    if(_value == HIGH && _heartbeat == true){
        int waitTime = 0;
        if(interval == 4){
            waitTime = 2000;
        }else if(interval == 3){
            waitTime = 1500;
        }else if(interval == 2){
            waitTime = 1000;
        }else if(interval == 1){
            waitTime = 500;
        }
        if(checkWait(waitTime)){
            heartbeat();
        }
    }
}

bool Led::checkWait(int interval){
    if(millis() > (lasttime + interval)){
        return true;
    }
    return false;
}

void Led::heartbeat(){
    int value = digitalRead(_pin);
    digitalWrite(!_pin);
}