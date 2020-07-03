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

// Dati Fissi Trasmissione
#define GATEWAY 200
#define PIC_ID 5


char* num_converter(int num);
void concatenate( char* str3, char* str1, char* str2 );

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
void Uart_send_string(char *str);

//variabili globali
int PicId=167;  // Identificazione Pic
char numStrade=4; // Variabile che segna il numero delle strade che il sistema deve gestire
unsigned char received = 0;

/*
 * BYTE TRASMISSIONE (strToSend)
 * 1 - Byte di controllo
 * 2 - destinatario (chi deve ricevere la trasmissione)
 * 3 - mittente (id di chi trasmette)
 * 4 - id della strada in questione
 * 5 - codici di trasmissione:     - temperatura: 0 (0000)
                                   - umidità: 1 (0001)
                                   - pressione: 2 (0010)
                                   - colore semaforo: 4 (0011)
                                   - numero ciclomotori: 5 (0100)
                                   - numero automezzi: 6 (0101)
                                   - numero camion: 7 (0110)

 * 6 - valore da trasmettere
 
 */


char strToSend[8] = {0,0,0,0,0,0}; // Messaggio totale da mandare in seriale

unsigned char datoSeriale=0; // Dato ricevuto dalla seriale
int temporizzazioneSemaforo=20;

int count=0;
char contAuto=0; // Conteggio automobili
char contMoto=0; // Conteggio motoveicoli
char contCamion=0; // Conteggio autocarri
unsigned char oldBtn1=0, stat1=0, oldBtn2=0, stat2=0, oldBtn3=0, stat3=0; // Pulsanti simulazione veicoli
unsigned char scattoSemafori=0;
char statoSemafori[3]={"V","G","R"};

void main(void) {
    
    initPic();
    UART_init(9600);
    init_lcd();
    send_cmd(L_CLR);
    
    char semafori[numStrade];
    
    //Messaggio Demo
    strToSend[0]=2;
    strToSend[1]=GATEWAY;
    strToSend[2]=PIC_ID;
    strToSend[3]=0;
    strToSend[4]=0;
    strToSend[5]=35;
    Uart_send_string(strToSend);
    
    char i;
    //for(i=0;i<4;i++)
    //{
        //semafori[i]="R";
    //}
    
    semafori[0]=statoSemafori[0];
    semafori[1]=statoSemafori[2];
    semafori[2]=statoSemafori[2];
    semafori[3]=statoSemafori[2];
    
    while(1)
    {
        
        // Gestione Counter Veicoli
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
        
        // Gestione Scatto Semaforo
        if(scattoSemafori==1)
        {
            
            char semaforoVerde=0;// Variabile di controllo per lo scatto a verde di un singolo semaforo
            for(i=0;i<4;i++)
            {
                
                
                 if(semafori[i]==statoSemafori[0])//Cambiamento a Rosso di tutti i semafori Verdi
                {
                    semafori[i]=statoSemafori[1];
                    //Scrittura a schermo
                    __delay_ms(1000);
                    semafori[i]=statoSemafori[2];
                }
            
                
                
                if(semaforoVerde==0)// Controllo che nessun semaforo sia già diventato verde
                {
                    if(semafori[i]==statoSemafori[0])// Se il semaforo corrente è Verde allora modifico
                    {
                        if(i<3)// Controllo se sono alla fine dell'array del semaforo
                        {
                            // Cambio Stato da Rosso a Verde
                            if(semafori[i+1]==statoSemafori[2])
                            {
                                semafori[i+1]==statoSemafori[0];
                            }
                        }
                        else// Se sono alla fine dell'array allora devo cambiare a Verde il primo elemento
                        {
                            if(semafori[0]==statoSemafori[2])
                            {
                                semafori[0]==statoSemafori[0];
                            }
                        }
                        
                        semaforoVerde=1;// Cambiamento della variabile di controllo per segnalare che lo scatto a Verde è avvenuto
                    }
                }
            }
        }
        
        
        // Gestione Trasmissione d'invio
        if(scattoSemafori==1)
        {
            
            
            
            scattoSemafori=0;
        }
        
        
        
        if(received)
        {
            char stringa=datoSeriale;
            
            send_cmd(L_CLR);
          
            /*
             
             * Gestione del ricevimento delle temporizzazioni
             * 
             * 0 - mittente (id di chi trasmette)
             * 1 - id della strada in questione
             * 2 - codici di trasmissione:     - temporizzazione: 8 (0111)
             * 3 - valore da trasmettere
             
             */
            
            
            
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
            //Timer Regolato/ Temporizzazione Semaforo Da Gateway
            /*
             if Tempo==0
             scattoSemafori=1;
             */
            
            count = 0;
        }
   }
}


/*
 * BYTE TRASMISSIONE (strToSend)
 * 0 - Byte di controllo
 * 1 - destinatario (chi deve ricevere la trasmissione)
 * 2 - mittente (id di chi trasmette)
 * 3 - id della strada in questione
 * 4 - codici di trasmissione:     - temperatura: 0 (0000)
                                   - umidità: 1 (0001)
                                   - pressione: 2 (0010)
                                   - colore semaforo: 4 (0011)
                                   - numero ciclomotori: 5 (0100)
                                   - numero automezzi: 6 (0101)
                                   - numero camion: 7 (0110)

 * 5 - valore da trasmettere
 
 */
void messageTransmission(char idStrada, char codice, char valore)
{
    strToSend[0]=2;
    strToSend[1]=GATEWAY;
    strToSend[2]=PIC_ID;
    strToSend[3]=0;
    strToSend[4]=0;
    strToSend[5]=35;
    Uart_send_string(strToSend);
}





char* num_converter(int num)
{
    int length = 2;
    char result[4] = "    ", i = 3;

    if(num != 0)
    {
        while(num)
        {
            result[i] = num%10 + '0';
            num /= 10;
            i--;
        }
    }
    else
    {
        result[0] = '0';
    }

    return result;
}


// Funzione per concatenare due stringhe
void concatenate( char* str3, char* str1, char* str2 )
{
    int i = 0, j = 0;
    while (str1[i] != '\0') { 
        str3[j] = str1[i];
        i++; 
        j++; 
    } 

    i = 0; 
    while (str2[i] != '\0') { 
        str3[j] = str2[i]; 
        i++; 
        j++; 
    } 
    str3[j] = '\0';
}


// Funzione per inviare l'array di byte alla seriale
void Uart_send_string(char *str)
{
    char i;
    for(i=0;i<6;i++)
    {
        UART_TxChar(*(str+i));
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