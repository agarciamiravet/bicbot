#include "DHT.h"  //Añadimos la libreria con la cual trabaja nuestro sensor
#define DHTPIN 2     // Indicamos el pin donde conectaremos la patilla data de nuestro sensor
#define DHTTYPE DHT11   // DHT 11 

DHT dht(DHTPIN, DHTTYPE);  //Indica el pin con el que trabajamos y el tipo de sensor
int maxh=0, minh=100, maxt=0, mint=100;  //Variables para ir comprobando maximos y minimos

int receivedChar;
boolean newData = false;
int ledHabitacion = 13;
int ledPasillo = 12;
int ledCocina = 11;
int ledBano = 10;

void setup() {

  Serial.begin(9600);

  pinMode(ledHabitacion, OUTPUT);
  pinMode(ledPasillo, OUTPUT);
  pinMode(ledCocina, OUTPUT);
  pinMode(ledBano, OUTPUT);

  dht.begin();
  
}

void loop() {

  recvInfo();
  lightLED();

  readSensorAgua();

  

  //readTemperature();
  
}

void readTemperature()
{
    // La lectura de la temperatura o de la humedad lleva sobre 250 milisegundos  
    // La lectura del sensor tambien puede estar sobre los 2 segundos (es un sensor muy lento)
    int h = dht.readHumidity();  //Guarda la lectura de la humedad en la variable float h
    int t = dht.readTemperature();  //Guarda la lectura de la temperatura en la variable float t
   
    // Comprobamos si lo que devuelve el sensor es valido, si no son numeros algo esta fallando
    if (isnan(t) || isnan(h)) // funcion que comprueba si son numeros las variables indicadas 
    {
      Serial.println("Fallo al leer del sensor DHT"); //Mostramos mensaje de fallo si no son numeros
    } else {
      //Mostramos mensaje con valores actuales de humedad y temperatura, asi como maximos y minimos de cada uno de ellos
      Serial.print("Humedad relativa: "); 
      Serial.print(h);
      Serial.print(" %\t");
      Serial.print("Temperatura: "); 
      Serial.print(t);
      Serial.println(" *C");
      //Comprobacion de maximos y minimos de humedad y temperatura
      if (maxh<h)
        maxh=h;
      if (h<minh)
        minh=h;
      if (maxt<t)
        maxt=t;
      if (t<mint)
        mint=t;
      Serial.print("Max: ");
      Serial.print(maxh);
      Serial.print(" % ");
      Serial.print("Min: ");
      Serial.print(minh);
      Serial.print(" %\t");
      Serial.print("Max: ");
      Serial.print(maxt);
      Serial.print(" *C ");
      Serial.print("Min: ");
      Serial.print(mint);
      Serial.println(" *C\n");
    }
    delay(1000);
}

void readSensorAgua()
{
  
  int waterMeassure = analogRead(A0);

  if (waterMeassure > 200)
  {
    Serial.println("WaterDetect");
    //Serial.println(analogRead(A0));
  }
  
}

void recvInfo() {

  if (Serial.available() > 0) {

    receivedChar = Serial.readString().toInt();
    newData = true;
    
  }
  
}

void lightLED() {

  int accion = receivedChar;

  while(newData == true) 
  {

    if(accion == 1)
    {
      digitalWrite(ledCocina, HIGH);
    }

    if(accion == 3)
    {
      digitalWrite(ledHabitacion, HIGH);
    }

    if(accion == 5)
    {
      digitalWrite(ledBano, HIGH);
    }

     if(accion == 7)
    {
      digitalWrite(ledPasillo, HIGH);
    }

    if(accion == 9)
    {
      digitalWrite(ledCocina, HIGH);
      digitalWrite(ledHabitacion, HIGH);
      digitalWrite(ledBano, HIGH);
      digitalWrite(ledPasillo, HIGH);
    }


    if(accion == 2)
    {
      digitalWrite(ledCocina, LOW);
    }

    if(accion == 4)
    {
      digitalWrite(ledHabitacion, LOW);
    }

    if(accion == 6)
    {
      digitalWrite(ledBano, LOW);
    }

     if(accion == 8)
    {
      digitalWrite(ledPasillo, LOW);
    }

    if(accion == 10)
    {
      digitalWrite(ledCocina, LOW);
      digitalWrite(ledHabitacion, LOW);
      digitalWrite(ledBano, LOW);
      digitalWrite(ledPasillo, LOW);
    }
    
    newData = false;
    
  }
  
}
