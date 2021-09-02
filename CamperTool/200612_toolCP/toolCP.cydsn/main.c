/*******************************************************************************
* File Name: main.c
*
* Version: 1.0
*
*******************************************************************************/
#include "debug.h"
#include "string.h"
#define checkEchod
#define command
#define keyboardd

uint8 cGetStatePrev;
uint32 recV,recB, check, checkB;
char dataBuf[50] = {};
//char dataTemp[50] = {};

int dataIndex = 0;


static char commandList[30][50]=
{
    "!cfg write ",
    "!ftm on\n",
    "!ftm sflash\n",
    "!ftm sflash2\n",
    "!ftm rfm_ex_1\n",
    "!ftm rfm_ex_5\n",
    "!ftm rfm_in\n",
    "!ftm gps\n",
    "!ftm gps2\n",
    "!ftm modem_imei\n",
    "!ftm modem_iccid\n",
    "!ftm modem_rssi\n",
    "!ftm modem_imsi\n",
    "!ftm modem_reg\n",
    "!cfg snWrite\n",   
    "!cfg init_config",
    "!ftm led\n",
    "!ftm led,success\n",
    "!ftm led,failure\n",
    "!pwroff 0\n",
    "!cfg init\n",    
    "",
    "!ftm debug_on\n",
    "!ftm debug_off\n",
    "!ftm mode_on\n",
    "!ftm fota_apn\n"
    ""
};
static char responseList[30][50]=
{
    "$SN,",        
    "$FTM,on",
    "$SFLASH,OK",
    "$ftm,sflash2",      //not used
    "$RFM_EX_1,OK",
    "$RFM_EX_5,OK",
    "$RFM_IN,OK",
    "$GPS,OK",
    "$GPS2,OK",         //test used[but not used]
    "$MODEM,IMEI,860536047698362",  
    "$MODEM,ICCID,87A90EDD11003669795F",
    "$MODEM,RSSI,66",
    "$MODEM,IMSI,99999999999",
    "$MODEM,REG,1",
    "$SN,",
    "$CFG_INIT,OK",
    "$LED,OK",
    "$LED,OK",
    "$LED,NOK",
    "$PWR,OFF",
    "$CFG_INIT,OK",
    ",OK",
    "$FTM,DEBUG,ON",
    "$FTM,DEBUG,OFF",
    "$FTM,ON",
    "$FOTA,APN,OK"
    ""
};

/*
static char negativeList[20][20]=
{
    "$ftm,info?",        //not used
    "$FTM,NOK",
    "$SFLASh,NOK",
    "$ftm,sflash2",      //not used
    "$RFM_EX,NOK",
    "$RFM_IN,NOK",
    "$GPS,NOK",
    "$GPS,gps2",         //test used[but not used]
    "$MODEM,VER",  
    "$MODEM,ICCID",
    "$MODEM,RSSI",
    "$MODEM,MDN",
    "$MODEM,REG",
    "$SN:",
    "$CFG_INIT,NOK",
    "$LED,NOK",
    "$PWR,OFF",
    "$complete",
    
    "","",    
};*/


void device_init()
{
    UART_Start();
    Debug_Print_Start(DEBUG_MESSAGE_LEVEL_3);
    Debug_Print(DEBUG_MESSAGE_LEVEL_3,"module start");
}

int main()
{    
    device_init();
    for(;;)
    {
        recV = UART_UartGetChar();
        
        if(recV > 0)
        {
            check = 0;
            dataBuf[dataIndex] = recV;
            dataIndex++;
        }
 
        
        if(check == 0)
        {
            switch(recV)
            {
               #ifdef keyboard
                case 49:
                    UART_UartPutString(responseList[1]);
                break;               
               
                case 50:
                    UART_UartPutString(responseList[2]);
                break;
                   
                case 51:
                    UART_UartPutString(responseList[3]);
                break;
                #endif
            
                #ifdef command
                case 10:
                   
                dataIndex = 0;
                check ++;    
                //Debug_Print(DEBUG_MESSAGE_LEVEL_3,"\r\ndataBuf:%s", dataBuf);    
                    
                if (strncmp(commandList[0], dataBuf, 10) == 0)
                //if (memcmp(dataBuf, commandList[0], strlen(dataBuf)) == 0)
                    {                        
                        char* dataPoint = strstr(dataBuf,commandList[0]);
                        char* dataTemp = strtok(dataPoint, "!cfg write");
                        
                        UART_UartPutString(responseList[0]);
                        UART_UartPutString(dataTemp);
                        UART_UartPutString(",OK");
                        break;
                    }
                    
                for (int i = 1; i < 30; i++)
                {
                    if (memcmp(dataBuf, commandList[i], strlen(dataBuf)) == 0)
                    {
                        //UART_UartPutString(" , ");    
                        UART_UartPutString(responseList[i]);
                        //Debug_Print(DEBUG_MESSAGE_LEVEL_3,"%s",responseList[i]);
                        //UART_UartPutString("\r\n");                        
                        break;
                    }
                }
                memset(dataBuf,0,strlen(dataBuf));
                break;         
                #endif
                
                #ifdef checkEcho
                default:
                
                    Debug_Print(DEBUG_MESSAGE_LEVEL_3,"%c", recV);
                    check ++;
                
                break;
                #endif
                
            }
        }
    }
}

/* [] END OF FILE */
