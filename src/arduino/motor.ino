
// Adafruit Motor shield library
// copyright Adafruit Industries LLC, 2009
// this code is public domain, enjoy!

#include <AFMotor.h>
int totalVentanaHabitacion =0;
int totalComedor =0;

int receivedChar;
boolean newData = false;

AF_DCMotor motor(1);



AF_DCMotor motorComedor(2);

void setup() {
  Serial.begin(9600);           // set up Serial library at 9600 bps
  
  // turn on motor
  motor.setSpeed(70);
  motor.run(RELEASE);

  motorComedor.setSpeed(70);
  motorComedor.run(RELEASE);
}

void loop() 
{
  recvInfo();
  CheckValue();

  //levantarVentanaHabitacion();
}

void CheckValue()
{
   int accion = receivedChar;

  while(newData == true) 
    {
      if(accion == 1)
      {
        //levantarVentanaHabitacion();

        for (int i=0; i <= 5200; i++)
        {
          motor.run(BACKWARD);
        } 
        motor.run(RELEASE);
      }

      if(accion == 2)
      {
        //levantarVentanaHabitacion();

        motor.setSpeed(70);
        for (int i=0; i <= 5400; i++)
        {
          motor.run(FORWARD);
        } 
        motor.run(RELEASE);
      }

      if(accion == 4)
      {
        //levantarVentanaHabitacion();

        for (int i=0; i <= 5200; i++)
        {
          motorComedor.run(BACKWARD);
        } 
        motorComedor.run(RELEASE);
      }

      if(accion == 5)
      {
        //levantarVentanaHabitacion();

        motorComedor.setSpeed(70);
        for (int i=0; i <= 5400; i++)
        {
          motorComedor.run(FORWARD);
        } 
        motorComedor.run(RELEASE);
      }
      
      newData = false;

      Serial.println("fin envio");
  }
}

void recvInfo() {

  if (Serial.available() > 0) 
  {
    Serial.println("recibido dato");
    receivedChar = Serial.read() - '0';
    newData = true;
    Serial.println(receivedChar);
    
    
  }
  
}



void levantarVentanaHabitacion()
{  
    Serial.println("levantar motor");
  
      if(totalVentanaHabitacion<30)
      {
        totalVentanaHabitacion++;
        motor.run(BACKWARD);
      }
      else
      {
        motor.run(RELEASE);
      }  
}


void levantarVentanaomedor()
{
    if(totalComedor<4700)
{
  totalComedor++;
  motorComedor.run(FORWARD);
}
else
{
  motorComedor.run(RELEASE);
}  
}


void bajarVentanaHabitacion()
{

    if(receivedChar == 1)
    {
         if(totalVentanaHabitacion<8000)
          {
            motor.setSpeed(50);
            totalVentanaHabitacion++;
            motor.run(FORWARD);
          }
          else
          {
            motor.run(RELEASE);
          } 
    }
}

