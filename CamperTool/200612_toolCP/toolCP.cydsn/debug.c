/*****************************************************************************
* File Name: debug.c
*
* Version: 1.0
*
* Description:
* This file contains Debug API
*
* Note:
* Define ENABLE_DEBUG_PIN in platform.h to enable macro for toggling the debug_pin
* Define DEBUG_PRINT in platform.h to enable debug print module
* Define SOFTWARE_UART in platform.h to enable debug print over Software Uart 
* else 'SCB configured as UART' is enabled 
*
*
* Related Document:
* UART component datasheet
*
*****************************************************************************/

#include "debug.h"

#ifdef DEBUG_PRINT

static uint8 debug_level;
/*****************************************************************************
* Function Name: Putc()
******************************************************************************
* Summary:
* This function is used to print a character on the UART 
*
* Parameters:
* uint8 ch - character value
*
* Return:
* None
*
* Theory:
* Send a character to the UART
*
* Side Effects:
* None 
*
* Note:
* None
*****************************************************************************/
static void Putc(char8 ch)
{
    /*This function has to be replaced by user */
#ifndef SOFTWARE_UART
    UART_UartPutChar(ch);
#else
    SW_Tx_UART_PutChar(ch);
#endif /* SOFTWARE_UART */
}

/*****************************************************************************
* Function Name: Byte_To_ASCII()
******************************************************************************
* Summary:
* This function is used to map the byte to its ASCII chacter value 
*
* Parameters:
* uint32 index - byte value
*
* Return:
* uint8 * - pointers to the ASCII character value
*
* Theory:
* Converts the byte into ascii character value using look-up table approach
*
* Side Effects:
* None 
*
* Note:
* None
*****************************************************************************/
static uint8* Byte_To_ASCII(uint32 Index)
{
    return (uint8*)("0123456789abcdef" + Index);
}

/*****************************************************************************
* Function Name: Debug_Print_Start()
******************************************************************************
* Summary:
* This function is used to initialize the debug functionality
*
* Parameters:
* uint8 level - Level for printing the debug message
*
* Return:
* None
*
* Theory:
* Starts the UART component and stores the debug_level for further procecssing
*
* Side Effects:
* debug_level is updated 
*
* Note:
* None
*****************************************************************************/
void Debug_Print_Start(uint8 level)
{
    debug_level = level;
}

/*****************************************************************************
* Function Name: Debug_Print()
******************************************************************************
* Summary:
* This function is used to print the logs on the UART 
*
* Parameters:
* uint8 level - Level of the debug message
* uint8 *pszFmt - Message to be print
*
* Return:
* None
*
* Theory:
* Steamlines the character to the UART and If there are any values it converts into the 
* ASCII and send it the UART
*
* Side Effects:
* None
*
* Note:
* The message will be printed if the level specified is above the debug level 
* set during the Debug_Start()
*****************************************************************************/
void Debug_Print(uint8 level, char8 *pszFmt,...)
{
    uint8 *pszVal;
    uint32 iVal, xVal, i = 0, buffer[12], index = 1;
    uint8 cVal;
    uint32 *pArg;
    pArg =(uint32 *)&pszFmt;
    if(level >= debug_level)
    {
        while(*pszFmt)
        {
            if('%' != *pszFmt)
            {
                Putc(*pszFmt);
                pszFmt++;
                continue;
            }
            pszFmt++;

            if(*pszFmt == 's')
            {
                pszVal = (uint8*)pArg[index++];
                for(; *pszVal != '\0'; pszVal++)
                    Putc(*pszVal);
                pszFmt++;
                continue;
            }
            if(*pszFmt == 'd')
            {
                iVal = pArg[index++];
                i = 0;
                do{
                    buffer[i++] = iVal % 10;
                    iVal /= 10;
                }while(iVal);
                while(i > 0)
                {
                    i--;
                    Putc(*Byte_To_ASCII(buffer[i]));
                }
                pszFmt++;
                continue;
            }
            if(*pszFmt == 'c')
            {
                cVal = pArg[index++];
                Putc(cVal);
                pszFmt++;
                continue;
            }
            if(*pszFmt == 'x')
            {
                xVal = pArg[index++];
                i = 0;
                do{
                    buffer[i++] = xVal % 16;
                    xVal /= 16;
                }while(xVal);
                if(i%2!=0)
                    buffer[i++]=0;
                if(i<2)
                    buffer[i++]=0;

                while(i > 0)
                {
                    i--;
                    Putc(*Byte_To_ASCII(buffer[i]));
                }
                pszFmt++;
                continue;
            }
            if(pszFmt == '\0')
            {
                break;
            }
        }
    }
}

#endif /* DEBUG_PRINT */
