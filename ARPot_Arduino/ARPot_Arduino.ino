#include <SoftwareSerial.h>

SoftwareSerial BT(2, 3); // RX, TX
int LED = 13;


void setup() {
  // Open serial communications and wait for port to open:
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }

  Serial.println("Serial Preparing!");

  // set the data rate for the SoftwareSerial port
  BT.begin(9600);
  //  BT.println("Hello, world?");

  pinMode(LED, OUTPUT);
  //  pinMode(sensorValue, INPUT);
}

void loop() { // run over and over
  int sensorValue = analogRead(A1);
  int sensorFlag = 500;
  Serial.println(sensorValue);

//    if (BT.available()) {
      Serial.println("BT Available");
      if (sensorValue >= sensorFlag) {
        BT.println("ON");
        Serial.println("ON");
        digitalWrite(LED, true);
      } else if (sensorValue < sensorFlag) {
        BT.println("OFF");
        Serial.println("OFF");
        digitalWrite(LED, false);
      } else {
        BT.write("NoData");
        digitalWrite(LED, false);
      }
      delay(500);
//    }

}
