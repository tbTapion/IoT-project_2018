#include "Arduino.h"
#include "Potmeter.h"

Potmeter::Potmeter(int pin){
    _pin = pin;
    _value = analogeRead(_pin);
    _previousValue = _value;
    _lasttime = millis();
    _timewait = 125;
}

int Potmeter::getValue(){
    return _value;
}

bool Potmeter::checkWait(){
    if(millis() > (_lasttime + _timewait)){
        _lasttime = millis();
        _value = analogeRead(_pin);
        if(_value != _previousValue){
            _previousValue = _value;
            return true;
        }
        return false;
    }
}