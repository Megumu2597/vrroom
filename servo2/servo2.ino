#include <Servo.h> 
 
const int servoPin = 3;        // サーボの接続ピン（定数）
const int mgservoPin = 4;
Servo myservo; // サーボオブジェクトを生成
Servo mgmyservo;
int pos=5 ;                   // サーボのポジション（変数）
//int posmg=0;
int sgnow;
int mgnow;
String inString = "";          // 受信文字列用のバッファ

void setup() 
{ 
//pinMode(LED_BUILTIN, OUTPUT);
  
  Serial.begin(9600);
 //mg90s
 mgmyservo.attach(mgservoPin,254,2300);   // サーボ変数をピンに割り当て
 //sg90s
 myservo.attach(servoPin,544,2480);
 myservo.write(30);         // ポジションを0に設定
  delay(15);                  // 回転するまで待つ
  sgnow=30;
  mgmyservo.write(90);         // ポジションを0に設定
  delay(15);
  mgnow=90; 
} 
 
void loop() 
{
  while (Serial.available() >0) {  // 受信データがあったら… //whileだったし>0だった
    int inChar = Serial.read();     // 1バイト読み込む 
   /*
    if (isDigit(inChar)) {           // 数値だったら…
      inString += (char)inChar;      // 文字列を連結する
    }
    if (inChar == '\n') {            // 改行コードLFが来たら…
      pos = inString.toInt();        // 文字列を数値に変換する
      if (pos < 0 || pos > 180) {    // 数値が範囲外なら…　//1だった
        Serial.println("Parameter Error");// エラー表示   
      }
      */
      inString=(char)inChar;
      if(inString.toInt() != 0){
      pos=inString.toInt();
       inString = "";                 // バッファクリア
    //}
    }}
        if (pos==3 && sgnow<55){
          myservo.write(sgnow+1);
          sgnow =sgnow+1;
          delay(100);//250
          
      }else if(pos==4 && sgnow>10){
            myservo.write(sgnow-1);
          sgnow =sgnow-1;
          delay(100);
          }
          else if(pos==1 &&mgnow>10){
            mgmyservo.write(mgnow-1);
          mgnow =mgnow-1;
          delay(100);
          }
          else if(pos==2 && mgnow<170){
            mgmyservo.write(mgnow+1);
          mgnow =mgnow+1;
          delay(100);
          }
          else{}
          
          //if (pos==1){ Serial.print("off\n"); digitalWrite(LED_BUILTIN,LOW); }
          //else if (pos==2){ Serial.print("on\n"); digitalWrite(LED_BUILTIN,HIGH); }
        /*
        //else if(sgnow != pos){
         // myservo.write(pos);   
          //sgnow=pos ;// 範囲内ならサーボモータを回転
          //delay(250);} 
        //myservo.write(pos-1);  // 回転するまで待つ
        //delay(200);
        //myservo.write(pos-2);  // 回転するまで待つ
        //delay(200);
        //Serial.print("OK:");        // OK
        Serial.println(mgnow);
        */
         //Serial.println(pos);
         Serial.println(mgnow);
      
     
  
}
