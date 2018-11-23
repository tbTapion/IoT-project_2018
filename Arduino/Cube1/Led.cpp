#include "Arduino.h"
#include "Led.h"

Led::Led(int pin){
    _pin = pin;
    _value = LOW;
    _lasttime = millis();
    _intervalBase = 500;
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
    _interval = interval - 48;
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
        waitTime = _interval * _intervalBase; // values 1-4 * 500 base
        if(checkWait(waitTime)){
            Serial.println("Beating!");
            heartbeat();
        }
    }
}

bool Led::checkWait(int interval){
    Serial.println("Checking wait!");
    if(millis() > (_lasttime + interval)){
        Serial.println("Wait over!");
        _lasttime = millis();
        return true;
    }
    return false;
}

void Led::heartbeat(){
    int value = digitalRead(_pin);
    digitalWrite(_pin, !value);
}
