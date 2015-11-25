#define ECHOPIN 2                           
#define TRIGPIN 7    
#define COMMAND 6   

#include <Servo.h>

int incomingByte = 0;

Servo monServo;

void setup(){
  Serial.begin(9600);
  pinMode(ECHOPIN, INPUT);
  pinMode(TRIGPIN, OUTPUT);
  monServo.attach(6);
  monServo.write(90);
}

void loop(){
  digitalWrite(TRIGPIN, LOW);                   // Trigger de 2us
  delayMicroseconds(2);
  digitalWrite(TRIGPIN, HIGH);                  // Send a 10uS high to trigger ranging
  delayMicroseconds(10);
  digitalWrite(TRIGPIN, LOW);                   // Send pin low again
  int distance = pulseIn(ECHOPIN, HIGH);        // Read in times pulse
  distance= distance/58;                        // Calculate distance from time of pulse
  Serial.print(distance); 
  if (Serial.available() > 0) {
                // read the incoming byte:
                incomingByte = Serial.read() ;
                if (incomingByte == 0) {
                monServo.write(170);
                } else if (incomingByte == 1)
                monServo.write(0);

                 //Serial.println(incomingByte); 
             
      
        }
  Serial.flush();             
  delay(50);                                    // Wait 50mS before next ranging
}
