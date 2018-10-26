#include "Arduino.h"
#include "Led.h"

Led::Led(int pin){
    _pin = pin;
    _value = 0;
    pinMode(_pin, OUTPUT);
}

void Led::setValue(int value){
    _value = value;
    digitalWrite(_pin, _value);
}

int Led::getValue(){
    return _value;
}