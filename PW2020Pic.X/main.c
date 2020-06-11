/*
 * File:   main.c
 * Author: PC Marco
 *
 * Created on 11 giugno 2020, 14.16
 */


#pragma config FOSC = HS        // Oscillator Selection bits (HS oscillator)
#pragma config WDTE = OFF       // Watchdog Timer Enable bit (WDT disabled)
#pragma config PWRTE = ON       // Power-up Timer Enable bit (PWRT enabled)
#pragma config BOREN = ON       // Brown-out Reset Enable bit (BOR enabled)
#pragma config LVP = ON         // Low-Voltage (Single-Supply) In-Circuit Serial Programming Enable bit (RB3/PGM pin has PGM function; low-voltage programming enabled)
#pragma config CPD = OFF        // Data EEPROM Memory Code Protection bit (Data EEPROM code protection off)
#pragma config WRT = OFF        // Flash Program Memory Write Enable bits (Write protection off; all program memory may be written to by EECON control)
#pragma config CP = OFF         // Flash Program Memory Code Protection bit (Code protection off)

#include <xc.h>
#define _XTAL_FREQ 32000000L

//LCD
# define L_ON 0x0F
# define L_OFF 0x08
# define L_CLR 0x01
# define L_L1 0x80 //prima riga del display
# define L_L2 0xC0 //seconda riga del display
# define L_CR 0x0F
# define L_NCR 0x0C
# define L_CFG 0x38
# define L_CUR 0x0C
#define EN PORTEbits.RE1
#define RS PORTEbits.RE2
#define LCDPORT PORTD

//UART
# define RC6 PORTCbits.RC6
# define RC7 PORTCbits.RC7


void initPic();

//LCD
void send_string(char *str);
void send_data(char data);
void send_cmd(char command);
void init_lcd();

//comunicazione seriale UART
void UART_init(int);
void UART_init(int baudrate);
void UART_TxChar(char dato);

//variabili globali
unsigned char received = 0;
int count=0;
void main(void) {
    
    initPic();
    UART_init(9600);
    init_lcd();
    send_cmd(L_CLR);
    
    while(1)
    {
        
        
        
        if(received)
        {
            received=0;
        }
    }
    
    return;
}



//Interrupt
void __interrupt() ISR()
{
   if(RCIF)
   {   
        while(!RCIF);        // ricevo dalla seriale:
        RCIF = 0;
        
        received = 1;
   }
   
   if (INTCON&0x04)
    {
        INTCON &= ~0x04;
        TMR0 = 6;
        count++;  
        if (count == 100)
        {
            
            count = 0;
        }
   }
}

//inizializzo il PIC
void initPic() {
    //TRISC = ~0x04;
    TRISD = 0x00; //TRISD output
    TRISB = 0xFF; //TRISB input
    TRISE = 0x00; //TRISE output
    //TRISCbits.TRISC0 = 0;
    INTCON = 0xA0;
    OPTION_REG = 0x04;
    TMR0 = 6;
}


//invio stringa a LCD
void send_string(char *str)
{
    int i = 0;
    while(str[i] != '\0')
    {
        send_data(str[i]);
        i++;
    }
}

//invio carattere a LCD
void send_data(char data)
{
    EN = 1;
    PORTD = data;
    RS = 1;
    __delay_ms(3);
    EN = 0;
    __delay_ms(3);
    EN = 1;
}

// invio comando a LCD
void send_cmd(char command)
{
    EN = 1;
    PORTD = command;
    RS = 0;
    __delay_ms(3);
    EN = 0;
    __delay_ms(3);
    EN = 1;
}

// inizializzazione LCD
void init_lcd()
{
    RS = 0;
    EN = 0;
    __delay_ms(20);
    EN = 1;
    send_cmd(L_CFG);
    __delay_ms(5);
    send_cmd(L_CFG);
    __delay_ms(1);
    send_cmd(L_CFG);
    send_cmd(L_OFF);
    send_cmd(L_ON);
    send_cmd(L_CLR);
    send_cmd(L_CUR);
    send_cmd(L_L1);
}


//inizializzazione comunicazione seriale UART
void UART_init(int baudrate)
{
    TRISCbits.TRISC6 = 0; 
    TXSTAbits.TXEN = 1;   
    RCSTAbits.SPEN = 1;   
    RCSTAbits.CREN = 1;   
    SPBRG = (_XTAL_FREQ/(long)(64UL*baudrate))-1;         
    INTCONbits.GIE = 1;  
    INTCONbits.PEIE = 1; 
    PIE1bits.RCIE = 1;      
}

//scrivo il carattere su terminale
void UART_TxChar(char dato) 
{
    while (!TXIF);
    TXIF = 0;
    TXREG = dato;
}