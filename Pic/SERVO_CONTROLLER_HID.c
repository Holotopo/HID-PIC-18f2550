//=========================================================================

#include <18F4550.h> // Definici?n de registros internos del PIC18F2550.
//#DEVICE ADC=8 // CAD a 8 bits, justificaci?n a a la derecha.
#DEVICE ADC=10 // CAD a 10 bits, justificaci?n a a la derecha.
//#DEVICE ADC=16 // CAD a 10 bits, justificaci?n a a la izquierda.

#fuses noMCLR,HSPLL,NOWDT,NOPROTECT,NOLVP,NODEBUG,USBDIV,PLL5,CPUDIV1,VREGEN
//#fuses HSPLL,NOWDT,NOPROTECT,NOLVP,NODEBUG,USBDIV,PLL5,CPUDIV1,VREGEN

// NOMCLR: No vamos ha usar el PIN MCLR, el reset se har? por software.
// HSPLL: Utilizaremos un cristal de alta velocidad en conjunto con el PLL.
// NOWDT: No vamos a usar el perro guardian.
// NOPROTECT: Memoria no protejida contra lecturas.
// NOLVP: No utilizamos el modo de programaci?n con bajo voltaje.
// NODEBUG: No utilizamos c?digo para debugear.
// USBDIV: signfica que el clock del usb se tomar? del PLL/2 = 96Mhz/2 = 48Mhz.
// PLL1: significa que el PLL prescaler dividir? la frecuencia del cristal. para HS = 20Mhz, PLL = 5.
//       con esto se obtiene: 20Mhz/5 = 4Mhz.
// CPUDIV1: El PLL postscaler decide la divisi?n en 2 de la frecuencia de salida del PLL de 96MHZ, si queremos 48MHZ, lo dejamos como est?.
// VREGEN: habilita el regulador de 3.3 volts que usa el m?dulo USB.
// NOPBADEN: Todo el Puerto B como I/O digitales.

// Usamos una frecuencia de trabajo de 48Mhz.
#use delay(clock=48000000)

//===========================================================================
#DEFINE USB_HID_DEVICE  TRUE

#define USB_EP1_TX_ENABLE  USB_ENABLE_INTERRUPT   //turn on EP1 for IN bulk/interrupt transfers
#define USB_EP1_TX_SIZE 64

#define USB_EP1_RX_ENABLE  USB_ENABLE_INTERRUPT   //turn on EP1 for IN bulk/interrupt transfers
#define USB_EP1_RX_SIZE 64


// Definiciones varias:
#define ON  output_high
#define OFF output_low
#define TB output_toggle


//========================================================================================================

// Incluimos librer?as utilizadas por la aplicaci?n.
#include <pic18_usb.h>                   // Drivers's USB del PIC18F2550.
#include "Descriptor_easyHID.h"          // Descriptores HID del proyecto.
#include <USB.c>                         // Funciones del USB.

// Usamos fast_io, en los puertos
#use fast_io(a)
#use fast_io(b)
#use fast_io(c) 
#use fast_io(d) 
#use fast_io(e)

// Variables globales.

    int8    recibe[64];                                    // Declaramos la variable recibe de 32 bytes.
    int8    envia[64];                                     // Declaramos la variable env?a  de 32 bytes.
    int8 duty1=150;//teorico entre 126 y 246, 120 pasos para el control del servo60
    int8 duty2=0;
    int8 duty3=0;
    int8 duty4=0;
    int8 duty5=0;
    int8 duty6=0;
    int8 duty7=0;
    int8 duty8=0;
    int8 duty9=0;
    int8 duty10=0;
    int8 duty11=0;
    int8 duty12=0;                                               // Variable que contiene el valor del PWM2.
    int16 valor,valor1,valor2;
    int8 centimetros,cuentas;
    
    int cont_pwm=1;//variables de control del pwm
    int8 cont_duty=7;//variable control de duty
    int1 flag; //variables de control del pwm
    int8 pulses=0;

//===========================================================================================================
void Usonic(){
cuentas=0;
 ON(pin_A4);
delay_us(10);
OFF(pin_A4);
while(input_state(pin_e3)==0&&cuentas<250){
cuentas++;
delay_us(58);
}
delay_ms(50);//demora de seguridad y retorna con el valor de centimetros contados
centimetros=cuentas;
}   

void usb_debug_task(void) {
      ON(PIN_C0);
      OFF(PIN_C1);
      usb_wait_for_enumeration(); // Espera a ser enumerado por el host.
      ON(PIN_C1);
      OFF(PIN_C0);
}


void config_adcon2(void) {
   #asm
   movlw 0b10111110 
   iorwf 0xFC0,1    
   #endasm
}

#int_RTCC
void  RTCC_isr(void)//funcion de interrupcion para control del pwm
{
  if (cont_pwm==0)
    {
      flag=1;
     cont_pwm=150;
    }
    cont_pwm--;
   clear_interrupt(INT_RTCC);

}

#int_TIMER2
void  TIMER2_isr(void) //timers de control para pwm con interrupcion
{
   if (flag)
   {
    switch(pulses)
   {
      case 0:
      {
         if(duty1!=0){
         setup_timer_2(T2_DIV_BY_16,duty1,cont_duty);         
         ON(pin_B7);}
         pulses=1;
      }
      break;
      case 1:
      {
         if(duty2!=0){
          setup_timer_2(T2_DIV_BY_16,duty2,cont_duty);
         ON(pin_B6);}
         OFF(pin_B7);
         pulses=2;
      }
      break;
      case 2:
      {
         if(duty3!=0){
         setup_timer_2(T2_DIV_BY_16,duty3,cont_duty);
         ON(pin_B5);}
         OFF(pin_B6);
         pulses=3;
      }
      break;
      case 3:
      {
      if(duty4!=0){
          setup_timer_2(T2_DIV_BY_16,duty4,cont_duty);
         ON(pin_B4);}
         OFF(pin_B5);//b5
         pulses=4;
      }
      break;
      case 4:
      {
      if(duty5!=0){
          setup_timer_2(T2_DIV_BY_16,duty5,cont_duty);
         ON(pin_B3);}
         OFF(pin_B4);
         pulses=5;
      }
      break;
      case 5:
      {
      if(duty6!=0){
         setup_timer_2(T2_DIV_BY_16,duty6,cont_duty);
         ON(pin_B2);}
         OFF(pin_B3);
         pulses=6;
      }
      break;
      case 6:
      {
      if(duty7!=0){
         setup_timer_2(T2_DIV_BY_16,duty7,cont_duty);
         ON(pin_B1);}
         OFF(pin_B2);
         pulses=7;
      }
      break;
      case 7:
      {
      if(duty8!=0){
         setup_timer_2(T2_DIV_BY_16,duty8,cont_duty);
         ON(pin_B0);}
         OFF(pin_B1);
         pulses=8;
      }
      break;
     case 8:
      {
      if(duty9!=0){
         setup_timer_2(T2_DIV_BY_16,duty9,cont_duty);
         ON(pin_D7);}
         OFF(pin_B0);
         pulses=9;
      }
      break;
      case 9:
      {
      if(duty10!=0){
         setup_timer_2(T2_DIV_BY_16,duty10,cont_duty);
         ON(pin_D6);}
         OFF(pin_D7);
         pulses=10;
      }
      break;
      case 10:
      {
      if(duty11!=0){
         setup_timer_2(T2_DIV_BY_16,duty11,cont_duty);
         ON(pin_D5);}
         OFF(pin_D6);
         pulses=11;
      }
      break;
      case 11:
      {
      if(duty12!=0){
         setup_timer_2(T2_DIV_BY_16,duty12,cont_duty);
         ON(pin_D4);}
         OFF(pin_D5);
         pulses=12;
      }
      break;
      case 12:
   {
         OFF(pin_D4);
         pulses=0;
         flag=0;
         cont_pwm=250;
   } 
    break;
   }
   }
   else {
      clear_interrupt(int_TIMER2);
   }
}

   
//===========================================================================================================




void main(void)                                                         // Funci?n Principal.
{
    
    // Configuraciones varias.
    set_tris_a(0x0F);
    set_tris_b(0x00);
    set_tris_d(0x00);                                                   // todo salidas
    set_tris_c(0x00);
    set_tris_e(0b1100);
    output_b(0x00);                                                     // Inicializamos las salidas a 0.
    output_d(0x00);    
    output_c(0x00);
    output_a(0x00); 
    output_e(0x00); 
    
    setup_adc (adc_clock_div_32); //Enciende ADC
    setup_adc_ports (an0_to_an3); //Elige terminales an?logicas
    config_ADCON2();

    setup_timer_0(RTCC_INTERNAL|RTCC_DIV_1|RTCC_8_bit); // configurar divisor de frec
    set_timer0(56);  
    setup_timer_2(T2_DIV_BY_1,1,1);  // seleccion de frecuencia para el duty
    
    enable_interrupts(GLOBAL); 
    enable_interrupts(INT_RTCC);
    enable_interrupts(INT_TIMER2);                                                    // Inicializamos el stack USB.
    usb_init();                   
     usb_task();   // Habilita el periferico usb y las interrupciones.
     usb_debug_task();
     
    while (TRUE)                                                        // Bucle infinito.
    {
        
            if(usb_enumerated())                                            // Si el dispositivo est? configurado...
        {
                            
              set_adc_channel (0); //Elige canal a medir
               valor=read_adc();
               envia[0]= make8(valor,0);
               envia[1]= make8(valor,1);
                delay_ms(5);
               set_adc_channel (1); //Elige canal a medir
               valor1=read_adc();
               envia[2]= make8(valor1,0);
               envia[3]= make8(valor1,1);
               delay_ms(5);

               set_adc_channel (2); //Elige canal a medir
               valor2=read_adc();
               envia[4]= make8(valor2,0);
               envia[5]= make8(valor2,1);
               delay_ms(5);
              // asigna valor de cm
               envia[6]=centimetros;
               envia[7]= flag;
               
              envia[8]=1; //life bit                  
               
              usb_put_packet(1, envia,32, USB_DTS_TOGGLE);      // Enviamos el paquete de datos por USB.
        
            if (usb_kbhit(1))                                           // Si hay un paquete de datos del host.. en el buffer lo tomamos y guardamos en la variable data.
            {
                usb_get_packet(1, recibe,32);      // En el buffer lo tomamos del EP1 y lo guardamos en la variable recibe....
    if(recibe[0]==1){}             
 //selector de duty

 duty1=recibe[1];
 duty2=recibe[2];
 duty3=recibe[3];
 duty4=recibe[4];
 duty5=recibe[5];
 duty6=recibe[6];
 duty7=recibe[7];
 duty8=recibe[8];
 duty9=recibe[9];
 duty10=recibe[10];
 duty11=recibe[11];
 duty12=recibe[12];
 //fin de duty

 //CONTROL DE LEDS EN LAS PATAS
 OFF(PIN_D3);
 if(recibe[13]==1)
 ON(PIN_D3);

 OFF(PIN_D2);
 if(recibe[14]==1)
 ON(PIN_D2);
 
 OFF(PIN_D1);
 if(recibe[15]==1)
ON(PIN_D1);
 
 OFF(PIN_D0);
 if(recibe[16]==1)
 ON(PIN_D0);
  
 OFF(PIN_c0);
 if(recibe[17]==1)
 ON(PIN_c0);
 
 OFF(PIN_c1);
 if(recibe[18]==1)
 ON(PIN_c1);
 
 OFF(PIN_c2); 
 if(recibe[19]==0x01)
 ON(PIN_c2);                               // Tomamos el dato y lo procesamos.
 
  if(recibe[20]==1){
  //Usonic();
  
   }
        }
    }
}
}                   
