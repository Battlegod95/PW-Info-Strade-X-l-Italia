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
#define PIC_ID 4 //Id del pic che identifica anche l'incrocio
#define TEMPO_DEFAULT 5

void messageTransmission(char tipoMessaggio, char idStrada, char codice, char valore);
void print_Countdown(int num, char statoSem);
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
 * 1 - Byte di controllo (se sensori 1 se veicoli o semaforo 2)
 * 2 - destinatario (chi deve ricevere la trasmissione)
 * 3 - mittente (id di chi trasmette)
 * 4 - id della strada in questione
 * 5 - codici di trasmissione:     - temperatura: 0 (0000)
                                   - umidità: 1 (0001)
                                   - pressione: 2 (0010)
                                   - colore semaforo: 3 (0011)
 *                                      valore 00000000 (0) = verde
 *                                      valore 00000001 (1) = giallo
 *                                      valore 00000010 (2) = rosso
                                   - numero ciclomotori: 4 (0100)
                                   - numero automezzi: 5 (0101)
                                   - numero camion: 6 (0110)

 * 6 - valore da trasmettere
 
 * 
 * 
 * GUARDARE LA TEMPORIZZAZIONE DEL GIALLO PER LEGGE
 * TEMPORIZZAZIONE MINIMA 3 secondi
 */


char strToSend[8] = {0,0,0,0,0,0}; // Messaggio totale da mandare in seriale

unsigned char datoSeriale=0; // Dato ricevuto dalla seriale

int count=0,f=3;
char contAuto=0; // Conteggio automobili
char contMoto=0; // Conteggio motoveicoli
char contCamion=0; // Conteggio autocarri
unsigned char oldBtn1=0, stat1=0, oldBtn2=0, stat2=0, oldBtn3=0, stat3=0; // Pulsanti simulazione veicoli
char semafori[4]; //Numero dei semafori dell'incrocio
unsigned char scattoSemafori=0,flagGiallo=0;
//Verde 0, Giallo 1, Rosso 2
char statoSemafori[3]={0,1,2};
char countDown=TEMPO_DEFAULT; // Temporizzazione del Verde, 35 secondi è la temporizzazione di default
char temporizzazione=0;

void main(void) {
    
    initPic();
    UART_init(9600);
    init_lcd();
    send_cmd(L_CLR);
    
    
    /*
    //Messaggio Demo temperatura: 0 (0000)
    strToSend[0]=2;
    strToSend[1]=GATEWAY;
    strToSend[2]=PIC_ID;
    strToSend[3]=0;
    strToSend[4]=0;
    strToSend[5]=35;
    Uart_send_string(strToSend);
    
    //Messaggio Demo umidità: 1 (0001)
    strToSend[0]=2;
    strToSend[1]=GATEWAY;
    strToSend[2]=PIC_ID;
    strToSend[3]=2;
    strToSend[4]=1;
    strToSend[5]=50;
    Uart_send_string(strToSend);
    //Messaggio Demo pressione: 2 (0010)
    strToSend[0]=2;
    strToSend[1]=GATEWAY;
    strToSend[2]=PIC_ID;
    strToSend[3]=4;
    strToSend[4]=2;
    strToSend[5]=1;
    Uart_send_string(strToSend);
    //Messaggio Demo colore semaforo: 4 (0011)
    strToSend[0]=2;
    strToSend[1]=GATEWAY;
    strToSend[2]=PIC_ID;
    strToSend[3]=0;
    strToSend[4]=4;
    strToSend[5]="R";
    Uart_send_string(strToSend);
    //Messaggio Demo numero ciclomotori: 5 (0100)
    strToSend[0]=2;
    strToSend[1]=GATEWAY;
    strToSend[2]=PIC_ID;
    strToSend[3]=0;
    strToSend[4]=5;
    strToSend[5]=5;
    Uart_send_string(strToSend);
    //Messaggio Demo numero automezzi: 6 (0101)
    strToSend[0]=2;
    strToSend[1]=GATEWAY;
    strToSend[2]=PIC_ID;
    strToSend[3]=2;
    strToSend[4]=6;
    strToSend[5]=4;
    Uart_send_string(strToSend);
    //Messaggio Demo numero camion: 7 (0110)
    strToSend[0]=2;
    strToSend[1]=GATEWAY;
    strToSend[2]=PIC_ID;
    strToSend[3]=3;
    strToSend[4]=7;
    strToSend[5]=2;
    Uart_send_string(strToSend);
    */
    
    /*
     - temperatura: 0 (0000)
                                   - umidità: 1 (0001)
                                   - pressione: 2 (0010)
                                   - colore semaforo: 4 (0011)
                                   - numero ciclomotori: 5 (0100)
                                   - numero automezzi: 6 (0101)
                                   - numero camion: 7 (0110)*/
    
    char i;
    
    //Inizializzazione Semafori
    semafori[1]=statoSemafori[2];
    semafori[2]=statoSemafori[2];
    semafori[3]=statoSemafori[2];
    semafori[0]=statoSemafori[0];
    
    
    while(1)
    {
        
        
        // Gestione Counter Veicoli
        if(stat1) // Auto
        {
            contAuto++;
            stat1=0;
            
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
            //Gestione dello scambio tra verde e rosso, ma non viceversa
            //Verde 0, Giallo 1, Rosso 2
            char semaforoVerde=0;// Variabile di controllo per lo scatto a verde di un singolo semaforo
            for(i=0;i<4;i++)
            {
                
                if(semaforoVerde==0)// Controllo che nessun semaforo sia già diventato verde
                {
                    if(semafori[i]==statoSemafori[0])// Se il semaforo corrente è Verde allora modifico
                    {
                        
                        if(semafori[i]==statoSemafori[0])//Cambiamento a Rosso di tutti i semafori Verdi
                        {
                            flagGiallo=1;
                            while(flagGiallo==1)
                                semafori[i]=statoSemafori[1]; 
                            //__delay_ms(3000); //3 Secondi Obbligatori di giallo
                            semafori[i]=statoSemafori[2];
                            
                            
                        }
                        
                        if(i<3)// Controllo se sono alla fine dell'array del semaforo
                        {
                            
                            // Cambio Stato da Rosso a Verde
                            if(semafori[i+1]==statoSemafori[2])
                            {
                                flagGiallo=1;
                                while(flagGiallo==1)
                                    semafori[i+1]=statoSemafori[1]; 
                                
                                
                                semafori[i+1]==statoSemafori[0];
                                
                            }
                        }
                        if(i>=3)// Se sono alla fine dell'array allora devo cambiare a Verde il primo elemento
                        {
                            if(semafori[0]==statoSemafori[2])
                            {
                                flagGiallo=1;
                                while(flagGiallo==1)
                                    semafori[0]=statoSemafori[1]; 
                                
                                semafori[0]==statoSemafori[0];
                                
                            }
                        }
                        
                        semaforoVerde=1;// Cambiamento della variabile di controllo per segnalare che lo scatto a Verde è avvenuto
                    }
                }
                
            }
            
            
        }
        
        //void messageTransmission(char tipoMessaggio, char idStrada, char codice, char valore);
        // Gestione Trasmissione d'invio
        if(scattoSemafori==1)
        {
            //Sensori
            messageTransmission(1, 1, 0, 10);// Simulare i sensori Temperatura
            messageTransmission(1, 2, 1, 60);//Sensore umidità
            messageTransmission(1, 4, 2, 1);//Sendore Pressione
            //Stato Semafori
            messageTransmission(2, 0, 3, semafori[0]);
            messageTransmission(2, 1, 3, semafori[1]);
            messageTransmission(2, 2, 3, semafori[2]);
            messageTransmission(2, 3, 3, semafori[3]);
            //Veicoli
            messageTransmission(2, 1, 4, contMoto);
            messageTransmission(2, 1, 4, 4);
            contMoto=0;
            messageTransmission(2, 1, 5, contAuto);
            messageTransmission(2, 1, 5, 7);
            contAuto=0;
            messageTransmission(2, 1, 6, 3);
            contCamion=0;
            scattoSemafori=0;
        }
        
        
        
        if(received)
        {
            //char stringa=datoSeriale;
            temporizzazione=datoSeriale;
            /*
             * Mi arriverà un numero e quel numero è la temporizzazione del verde
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
    
    
    if(f==0)
    {
        flagGiallo=0;
        f=3;
    }
    
    if(countDown==0)
    {
        scattoSemafori=1;
        if(temporizzazione!=0)
            countDown=temporizzazione;
        else
            countDown=TEMPO_DEFAULT;
    }
    
    
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
        if (count == 125) //Quando count diventa 125 è passato un secondo
        {
            //Aggiornamento dello stato visuale del semaforo con id 0
            if(semafori[0]==statoSemafori[0])
                print_Countdown(countDown, 0);//seguo il semaforo 0
            if(semafori[0]==statoSemafori[2])
                print_Countdown(countDown, 2);
            
            
            if(flagGiallo==1)
            {
                if(semafori[0]==statoSemafori[1])
                   print_Countdown(f, 1);
                
                f--;
                
            }
            else
            {
                countDown--;
                
            }
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
void messageTransmission(char tipoMessaggio, char idStrada, char codice, char valore)
{
    strToSend[0]=tipoMessaggio;
    strToSend[1]=GATEWAY;
    strToSend[2]=PIC_ID;
    strToSend[3]=idStrada;
    strToSend[4]=codice;
    strToSend[5]=valore;
    Uart_send_string(strToSend);
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
    OPTION_REG = 0x07;
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

void print_Countdown(int num, char statoSem)
{
    char resultNum[3], firstStr[17] = "Tempo:          ";
    int length = 2, i = 0;
    
    if(num < 10) //Controllo la lunghezza del numero
        length = 1;
    else if(num == 100)
        length = 3;
    
    if(num != 0)
    {
        while(num) //Assegno ogni cifra del numero alla stringa accondando il carattere '0' cosi' da renderlo un carattere
        {
            resultNum[i] = num%10 + '0';
            num /= 10;
            i++;
        }   
    }
    else
    {
        resultNum[0] = '0'; //Se il numero e' 0 allora lo inserisco direttamente senno' da errore e non stampa nulla a schermo
    }
    
    for(i=0; i<length; i++) //Sostituisco i campi vuoti con la stringa del numero che ho trovato
    {
        firstStr[13-i] = resultNum[i];
    }
    
    send_cmd(L_CLR); //Pulisco il display
    send_cmd(L_L2);
    if(statoSem==0)//Verde
    {
       char secondStr[17] = "Sem 0:     Verde";
       send_string(secondStr);
    }
    if(statoSem==1)//Giallo
    {
       char secondStr[17] = "Sem 0:    Giallo";
       send_string(secondStr);
    }
    if(statoSem==2)//Rosso
    {
       char secondStr[17] = "Sem 0:     Rosso";
       send_string(secondStr);
    }
    
    //send_string(secondStr);
    __delay_ms(100);
    
    send_cmd(L_L1); //Punto alla riga desiderata
    send_string(firstStr); //Stampo la temperatura
    
    
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