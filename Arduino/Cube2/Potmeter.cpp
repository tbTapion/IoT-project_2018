#include "Arduino.h"
#include "Potmeter.h"

Potmeter::Potmeter(int pin){
    _pin = pin;
    _value = analogRead(_pin);
    _previousValue = _value;
    _lasttime = millis();
    _timewait = 125;
}

int Potmeter::getValue(){
    Serial.println(String(_value).c_str());
    return _value;
}

bool Potmeter::checkWait(){
    if(millis() > (_lasttime + _timewait)){
        _lasttime = millis();
        _value = analogRead(_pin);
        //Serial.print(_value);
        if(_value+5 < _previousValue || _value-5 > _previousValue){
            _previousValue = _value;
            return true;
        }
        return false;
    }
    return false;
}
