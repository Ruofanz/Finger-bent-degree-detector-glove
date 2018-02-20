
/*
  Analog input, analog output, serial output
 
 Reads an analog input pin, maps the result to a range from 0 to 255
 and uses the result to set the pulsewidth modulation (PWM) of an output pin.
 Also prints the results to the serial monitor.
 
 The circuit:
 * potentiometer connected to analog pin 0.
   Center pin of the potentiometer goes to the analog pin.
   side pins of the potentiometer go to +5V and ground
 * LED connected from digital pin 9 to ground
 
 created 29 Dec. 2008
 modified 9 Apr 2012
 by Tom Igoe
 
 This example code is in the public domain.
 
 */

// These constants won't change.  They're used to give names
// to the pins used:
#include<stdlib.h>
const int analogInPin = A0;  // Analog input pin that the potentiometer is attached to
const int analogOutPin = 9; // Analog output pin that the LED is attached to

int sensorValue1 = 0;
int sensorValue2 = 0;
int sensorValue3 = 0;
int sensorValue4 = 0;
// value read from the pot
String sensorData="";
char s1 = 0;
char s2 = 0;
char s3 = 0;
char s4 = 0;

int outputValue = 0;        // value output to the PWM (analog out)

void setup() {
    sensorValue1 = 21;
  sensorValue2 = 32;
  sensorValue3 = 55;
  sensorValue4 = 67;
  // initialize serial communications at 9600 bps:
  Serial.begin(9600); 
  randomSeed(100);
}

void loop() {
         
  

  // print the results to the serial monitor:
sensorValue1 = random(99);
sensorValue2 = random(99);
sensorValue3 = random(99);  
sensorValue4 = random(99);

if(sensorValue1<=9)
{
sensorData.concat(0);  
sensorData.concat(sensorValue1);
}
else
{
  sensorData.concat(sensorValue1);
}

if(sensorValue2<=9)
{
sensorData.concat(0);  
sensorData.concat(sensorValue2);
}
else
{
  sensorData.concat(sensorValue2);
}

if(sensorValue3<=9)
{
sensorData.concat(0);  
sensorData.concat(sensorValue3);
}
else
{
  sensorData.concat(sensorValue3);
}

if(sensorValue4<=9)
{
sensorData.concat(0);  
sensorData.concat(sensorValue4);
}
else
{
  sensorData.concat(sensorValue4);
}

Serial.println(sensorData); 
delay(300);
sensorData = "";
  // wait 2 milliseconds before the next loop
  // for the analog-to-digital converter to settle
  // after the last reading:
                     
}
