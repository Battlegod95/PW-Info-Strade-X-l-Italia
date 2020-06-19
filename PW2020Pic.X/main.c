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


void printOnLcd(char *datoFromSerial);


void initPic();

//ADC
void init_ADC();
int read_ADC(char channel);

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
char PicId=167;  // Identificazione Pic
unsigned char received = 0;
unsigned char datoSeriale=0; // Dato ricevuto dalla seriale
int count=0;
char contAuto=0; // Conteggio automobili
char contMoto=0; // Conteggio motoveicoli
char contCamion=0; // Conteggio autocarri
unsigned char oldBtn1=0, stat1=0, oldBtn2=0, stat2=0, oldBtn3=0, stat3=0; // Pulsanti simulazione veicoli

void main(void) {
    
    initPic();
    UART_init(9600);
    init_lcd();
    send_cmd(L_CLR);
    
    //UART_TxChar(PicId);  
    
    while(1)
    {
        
        
        if(stat1) // Auto
        {
            contAuto++;
            stat1=0;
            UART_TxChar(contAuto);
        }
        if(stat2) // Moto
        {
            contMoto++;
            stat2=0;
        }
        if(stat3) // Camion
        {
            contCamion++;
            stat3=0;
        }
        
        
        
        if(received)
        {
            char stringa=datoSeriale;
            
            send_cmd(L_CLR);
            //send_string(stringa);
            
            printOnLcd(datoSeriale);
            
            
            received=0;
        }
    }
    
    return;
}



//Interrupt
void __interrupt() ISR()
{
    
    //Pulsante per la simulazione del passaggio di autoveicoli
    if (!(PORTBbits.RB3)&& (!oldBtn1))
    {
        stat1=!stat1;
            
    }
    oldBtn1 = !PORTBbits.RB3;
    //Pulsante per la simulazione del passaggio di motoveicoli
    if (!(PORTBbits.RB4)&& (!oldBtn2))
    {
        stat2=!stat2;
            
    }
    oldBtn2 = !PORTBbits.RB4;
    //Pulsante per la simulazione del passaggio di autocarri
    if (!(PORTBbits.RB5)&& (!oldBtn3))
    {
        stat3=!stat3;
            
    }
    oldBtn3 = !PORTBbits.RB5;
    
    
    
   if(RCIF)
   {   
        while(!RCIF);        // ricevo dalla seriale:
        RCIF = 0;
        datoSeriale = RCREG;
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

//Formatta e stampa il dato in arrivo dalla seriale
void printOnLcd(char *datoFromSerial)
{
    char resultNum[3], resultStr[9];
    
    resultStr=datoFromSerial;
    
    int i = 0;
    while(datoFromSerial[i] != '\0')
    {
        resultStr[i]=datoFromSerial[i];
        i++;
    }
    
     
    send_cmd(L_CLR); //Pulisco il display
    
    send_string(resultStr); //Stampo la temperatura
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

void init_ADC()
{
    TRISA = 0xFF;    //Imposto i pin come ingressi
    ADCON0 = 0x00;   //Setto ADCON0 00000000
    ADCON1 = 0x80;   //Setto ADCON1 (ADFM) a 1 --> risultato giustificato verso dx 10000000
    
    __delay_us(10);  //delay condensatore 10us
}

int read_ADC(char canale)
{
    ADCON0bits.ADON = 1;          //Accendo il convertittore (ADCON0)
    ADCON0 |= canale << 3;       //Setto il canale da convertire (ADCON0)
    
    __delay_us(1.6);     //Attendo 1.6 uS
    
    GO_nDONE = 1;      //Avvio la conversione ADGO GO
    
    while(GO_nDONE);  //Attendo la fine della conversione
    return ADRESL + (ADRESH << 8); //Preparo il dato (valore = ADRESL + (ADREAH << 8)
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