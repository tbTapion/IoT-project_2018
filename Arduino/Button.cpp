#include "Arduino.h"
#include "Button.h"

#define DEBOUNCE 10;

Button::Button(int pin){
    _pin = pin;
    _pressed = 0; 
    _checked = 1;
    _currentState = LOW;
    _previousState = LOW;
    _lastTime = millis();
    pinMode(_pin, INPUT);
}

void Button::update(){
    //Check debounce time and reset if enough time has passed
    if((_lastTime + DEBOUNCE) > millis()){
        return;
    }
    _lastTime = millis();

    _currentState = digitalRead(_pin);

    if(_currentState != _previousState){
        _pressed = _currentState
    }
}

boolean Button::pressed() {
}
boolean Button::lifted(){

}