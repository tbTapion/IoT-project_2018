#include "Arduino.h"
#include "Potmeter.h"

Potmeter::Potmeter(int pin){
    _pin = pin;
    _value = analogRead(_pin);
    _previousValue = _value;
    _lasttime = millis();
    _timewait = 125;
}

char* Potmeter::getValue(){
    char buff [10];
    itoa(_value, buff, 10); 
    return buff;
}

bool Potmeter::checkWait(){
    if(millis() > (_lasttime + _timewait)){
        _lasttime = millis();
        _value = analogRead(_pin);
        Serial.println(_value);
        if(_value+3 < _previousValue || _value-3 > _previousValue){
            _previousValue = _value;
            return true;
        }
        return false;
    }
    return false;
}
