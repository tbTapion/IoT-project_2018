#include <MPU9250_RegisterMap.h>
#include <SparkFunMPU9250-DMP.h>
#include <Adafruit_NeoPixel.h>
#include "imumaths.h"


#include <Wire.h>
#include "Adafruit_VL6180X.h"
#include <Adafruit_NeoPixel.h>

Adafruit_VL6180X sensor = Adafruit_VL6180X();

/************************************************************
MPU9250_DMP_Quaternion
 Quaternion example for MPU-9250 DMP Arduino Library 
Jim Lindblom @ SparkFun Electronics
original creation date: November 23, 2016
https://github.com/sparkfun/SparkFun_MPU9250_DMP_Arduino_Library

The MPU-9250's digital motion processor (DMP) can calculate
four unit quaternions, which can be used to represent the
rotation of an object.

This exmaple demonstrates how to configure the DMP to 
calculate quaternions, and prints them out to the serial
monitor. It also calculates pitch, roll, and yaw from those
values.

Development environment specifics:
Arduino IDE 1.6.12
SparkFun 9DoF Razor IMU M0

Supported Platforms:
- ATSAMD21 (Arduino Zero, SparkFun SAMD21 Breakouts)
*************************************************************/

// DS 2018: New version of library that works on AVR architectures...

#define SerialPort Serial

MPU9250_DMP imu;

#define PIN 2 

const int no_of_Pixels = 24;
const int north_Pixel = 0;
int east_Pixel = no_of_Pixels / 4;
int south_Pixel = east_Pixel * 2;
int west_Pixel = east_Pixel * 3;

Adafruit_NeoPixel strip = Adafruit_NeoPixel(no_of_Pixels, PIN, NEO_GRB + NEO_KHZ800);

void setup() 
{
  SerialPort.begin(9600);
  SerialPort.println("Start.");

    if (! sensor.begin()) {
    Serial.println("Failed to find sensor.");
    while (1);
  }
  Serial.println("Sensor found!");
  
  // Call imu.begin() to verify communication and initialize
  if (imu.begin() != INV_SUCCESS)
  {
    while (1)
    {
      SerialPort.println("Unable to communicate with MPU-9250");
      SerialPort.println("Check connections, and try again.");
      SerialPort.println();
      delay(7000);
    }
  }
    SerialPort.println("Begin 1.");
    int success = imu.dmpBegin(DMP_FEATURE_6X_LP_QUAT | // Enable 6-axis quat
              DMP_FEATURE_GYRO_CAL | DMP_FEATURE_TAP, // Use gyro calibration
              10); // Set DMP FIFO rate to 10 Hz
  // DMP_FEATURE_LP_QUAT can also be used. It uses the 
  // accelerometer in low-power mode to estimate quat's.
  // DMP_FEATURE_LP_QUAT and 6X_LP_QUAT are mutually exclusive

  // dmpSetTap parameters, in order, are:
  // x threshold: 1-1600 (0 to disable)
  // y threshold: 1-1600 (0 to disable)
  // z threshold: 1-1600 (0 to disable)
  // (Threshold units are mg/ms)
  // taps: Minimum number of taps needed for interrupt (1-4)
  // tap time: milliseconds between valid taps
  // tap time multi: max milliseconds between multi-taps
  unsigned short xThresh = 0;   // Disable x-axis tap
  unsigned short yThresh = 0;   // Disable y-axis tap
  unsigned short zThresh = 20; // Set z-axis tap thresh to 100 mg/ms
  unsigned char taps = 1;       // Set minimum taps to 1
  unsigned short tapTime = 300; // Set tap time to 100ms
  unsigned short tapMulti = 1000;// Set multi-tap time to 1s
  imu.dmpSetTap(xThresh, yThresh, zThresh, taps, tapTime, tapMulti);



  
    SerialPort.print(success);
      SerialPort.println("Begin 2.");
  strip.begin();
  strip.show(); // Initialize all pixels to 'off'
  



}

bool firstTimeOff = false;
uint8_t sensorStatus = -1;
bool rangeCode() {
  uint8_t range = sensor.readRange();
  sensorStatus = sensor.readRangeStatus();
  // Serial.print("Range: "); Serial.println(range);
  if (sensorStatus == VL6180X_ERROR_NONE) {   // Range between 0 and approx. 160 
     // Serial.print("Range: "); Serial.println(range);
     int pixels = map(range,15,160,no_of_Pixels,0);
     int red = map(range, 0, 160, 255, 0);
     int blue = map(range, 0, 160, 0, 255);
    // int green = map(range, 0, 160, 0, 255);
     pixelBar(&strip, pixels, strip.Color(red, 0, blue));
     firstTimeOff = true;
     return true; 
  } else {
     if (firstTimeOff) {
        pixelBar(&strip, 0, strip.Color(0, 0, 0));
        firstTimeOff = false;
     }
  }
  return false;
}

int rangeDelay = 300;
unsigned long rangeTime = -1;
void loop() {
  if (rangeTime == -1) {
    rangeTime = millis();
  } else {
    if (millis() > (rangeTime + rangeDelay)) {
      rangeCode();
      rangeTime = -1;
    }
  }

  if (sensorStatus == VL6180X_ERROR_NONE) {
    rangeDelay = 10;
    return;
  } else {
    rangeDelay = 300;
  }
  
  tap_loop();
  // Check for new data in the FIFO
  if ( imu.fifoAvailable() )
  {
    // SerialPort.println("loop2");
    // Use dmpUpdateFifo to update the ax, gx, mx, etc. values
    if ( imu.dmpUpdateFifo() == INV_SUCCESS) {
      // computeEulerAngles can be used -- after updating the
      // quaternion values -- to estimate roll, pitch, and yaw
      imu.computeEulerAngles();
      printIMUData();
      if ( imu.tapAvailable() ) {
         unsigned char tapDir = imu.getTapDir();
         unsigned char tapCnt = imu.getTapCount();
         if ((tapDir = TAP_Z_UP) || (tapDir = TAP_Z_DOWN)) {
            tap_start();
      //      SerialPort.print("Tap Z");
         }
      }
    }
  }
}

unsigned long blinkTime = -1;

void tap_start() {
  blinkTime = millis();
  tone(13,1000,100);
}

void tap_loop() {
  if ((blinkTime != -1)) {
    if (millis() > (blinkTime + 300)) {
      for (int i=no_of_Pixels;i>0;i--) {
        strip.setPixelColor(i, strip.Color(0, 0, 0));
      };
      blinkTime = -1;
    } else {
      for (int i=0;i<no_of_Pixels;i++) {
        strip.setPixelColor(i, strip.Color(0, 128, 0));
      }
    }
  }
}

bool first_reading = true;
float first_pitch = 0.0;
float first_roll = 0.0;

void printIMUData(void)
{  
  // After calling dmpUpdateFifo() the ax, gx, mx, etc. values
  // are all updated.
  // Quaternion values are, by default, stored in Q30 long
  // format. calcQuat turns them into a float between -1 and 1
  float q0 = imu.calcQuat(imu.qw);
  float q1 = imu.calcQuat(imu.qx);
  float q2 = imu.calcQuat(imu.qy);
  float q3 = imu.calcQuat(imu.qz);

  Quaternion q(q0,q1,q2,q3);
  Vector<3> qEuler = q.toEuler();
  Vector<3> qEulerDeg(qEuler.x() * 180.0 / M_PI, qEuler.y() * 180.0 / M_PI, qEuler.z() * 180.0 / M_PI);
 
  float pitchAngle = -qEulerDeg.y();
  float rollAngle  = -qEulerDeg.z();
  float yawAngle = qEulerDeg.x();

  /*
   if (first_reading) {
    first_reading = false;
    first_pitch = pitchAngle;
    first_roll = rollAngle;
   } else {
    pitchAngle = pitchAngle - first_pitch;
    rollAngle = rollAngle - first_roll;
   }
*/
  
 // SerialPort.println(" R/P/Y: " + String(rollAngle) + ", "
 //           + String(pitchAngle) + ", " + String(yawAngle));

  if (pitchAngle > 0.0) {
    int north = pitchAngle;
    strip.setPixelColor(north_Pixel, strip.Color(north, 0, 0));
    strip.setPixelColor(south_Pixel, 0);
  } else {
    int south = -pitchAngle;
    strip.setPixelColor(south_Pixel, strip.Color(south, 0, 0));
    strip.setPixelColor(north_Pixel, 0);
  }

  if (rollAngle > 0.0) {
    int east = rollAngle;
    strip.setPixelColor(east_Pixel, strip.Color(east, 0, 0));
    strip.setPixelColor(west_Pixel, 0);
  } else {
    int west = -rollAngle;
    strip.setPixelColor(west_Pixel, strip.Color(west, 0, 0));
    strip.setPixelColor(east_Pixel, 0);
  }
  strip.show();
}


// Set the first noOfPixelsOn pixels on stripBar to color.
// Mustbe called pixelBar(&strip, n, strip.Color(r, g, b));
// DS 2018...
void pixelBar(Adafruit_NeoPixel *stripBar, int noOfPixelsOn, uint32_t color) {
  int noOfPixelsTotal = stripBar->numPixels();
  // Serial.print("rgb: "); Serial.print(r); Serial.print(",");Serial.print(g); Serial.print(","); Serial.println(b);
  if ((noOfPixelsOn >= 0) && (noOfPixelsOn <= noOfPixelsTotal)) {
    for(uint16_t i=0; i<noOfPixelsOn; i++) {
        stripBar->setPixelColor(i, color);
        stripBar->show();
     }
   for(uint16_t i=noOfPixelsOn; i<noOfPixelsTotal; i++) {
      stripBar->setPixelColor(i, 0);
      stripBar->show();
   }
  } 
}
