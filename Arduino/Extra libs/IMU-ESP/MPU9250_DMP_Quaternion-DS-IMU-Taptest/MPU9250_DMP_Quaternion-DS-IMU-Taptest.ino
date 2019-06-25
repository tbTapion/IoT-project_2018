#define PROGMEM   ICACHE_RODATA_ATTR

#include <MPU9250_RegisterMap.h>
#include <SparkFunMPU9250-DMP.h>

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

void setup() 
{
  SerialPort.begin(9600);
  SerialPort.println("Start.");
  // Call imu.begin() to verify communication and initialize
  if (imu.begin() != INV_SUCCESS)
  {
    while (1)
    {
      SerialPort.println("Unable to communicate with MPU-9250");
      SerialPort.println("Check connections, and try again.");
      SerialPort.println();
      delay(5000);
    }
  }
    SerialPort.println("Begin 1.");
    int success = imu.dmpBegin(DMP_FEATURE_6X_LP_QUAT | // Enable 6-axis quat
              DMP_FEATURE_GYRO_CAL | DMP_FEATURE_TAP, // Use gyro calibration
              10); // Set DMP FIFO rate to 10 Hz
  // DMP_FEATURE_LP_QUAT can also be used. It uses the 
  // accelerometer in low-power mode to estimate quat's.
  // DMP_FEATURE_LP_QUAT and 6X_LP_QUAT are mutually exclusive

  unsigned short xThresh = 0;   // Disable x-axis tap
  unsigned short yThresh = 0;   // Disable y-axis tap
  unsigned short zThresh = 50; // Set z-axis tap thresh to 100 mg/ms
  unsigned char taps = 1;       // Set minimum taps to 1
  unsigned short tapTime = 100; // Set tap time to 100ms
  unsigned short tapMulti = 1000;// Set multi-tap time to 1s
  imu.dmpSetTap(xThresh, yThresh, zThresh, taps, tapTime, tapMulti);
  
    SerialPort.print(success);
      SerialPort.println("Begin 2.");

}

void loop() 
{

  // Check for new data in the FIFO
  if ( imu.fifoAvailable() )
  {
    // SerialPort.println("loop2");
    // Use dmpUpdateFifo to update the ax, gx, mx, etc. values
    if ( imu.dmpUpdateFifo() == INV_SUCCESS)
    {
      // computeEulerAngles can be used -- after updating the
      // quaternion values -- to estimate roll, pitch, and yaw
      imu.computeEulerAngles();
      printIMUData();
    }
       if ( imu.tapAvailable() )
    {
      // If a new tap happened, get the direction and count
      // by reading getTapDir and getTapCount
      unsigned char tapDir = imu.getTapDir();
      unsigned char tapCnt = imu.getTapCount();
      switch (tapDir)
      {
      case TAP_X_UP:
          SerialPort.print("Tap X+ ");
          break;
      case TAP_X_DOWN:
          SerialPort.print("Tap X- ");
          break;
      case TAP_Y_UP:
          SerialPort.print("Tap Y+ ");
          break;
      case TAP_Y_DOWN:
          SerialPort.print("Tap Y- ");
          break;
      case TAP_Z_UP:
          SerialPort.print("Tap Z+ ");
          break;
      case TAP_Z_DOWN:
          SerialPort.print("Tap Z- ");
          break;
      }
      SerialPort.println(tapCnt);
    }
  }
}

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

  SerialPort.println("R/P/Y: " + String(imu.roll) + ", "
            + String(imu.pitch) + ", " + String(imu.yaw));

}
