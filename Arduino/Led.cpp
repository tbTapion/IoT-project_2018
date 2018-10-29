#include "Arduino.h"
#include "Led.h"

Led::Led(int pin){
    _pin = pin;
    _value = LOW;
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
