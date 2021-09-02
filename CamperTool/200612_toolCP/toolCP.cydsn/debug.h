/*****************************************************************************
* File Name: debug.h
*
* Version: 1.0
*
* Description:
* This file contains Debug API functions.
*
* Note:
* Define ENABLE_DEBUG_PIN in platform.h to enable macro for toggling the debug_pin
* Define DEBUG_PRINT in platform.h to enable debug print module
* Define SOFTWARE_UART in platform.h to enable debug print over Software Uart 
* else SCB configured as UART is enabled 
*
* Owner: MRAO
* 
* Related Document:
* UART component datasheet
* SW_Tx_UART component datasheet
*
*****************************************************************************/
#ifndef _DEBUG_H_
#define _DEBUG_H_
#include <project.h>

//#define DEBUG_PRINT

/* Macros for the toggling the GPIO */
#ifdef ENABLE_DEBUG_PIN
#define DEBUG_PIN_LOW                   Debug_Pin_Write(0)
#define DEBUG_PIN_HIGH                  Debug_Pin_Write(1)
#define DEBUG_PIN_VALUE(x)              Debug_Pin_Write(x)
#define DEBUG_PIN_TOGGLE()              Debug_Pin_Write(!Debug_Pin_Read())
#else
#define DEBUG_PIN_LOW
#define DEBUG_PIN_HIGH
#define DEBUG_PIN_VALUE(x)
#define DEBUG_PIN_TOGGLE()       
#endif /* ENABLE_DEBUG_PIN  */

/* Different level of the debug message */
#define DEBUG_MESSAGE_LEVEL_0             (00u)
#define DEBUG_MESSAGE_LEVEL_1             (01u)
#define DEBUG_MESSAGE_LEVEL_2             (02u)
#define DEBUG_MESSAGE_LEVEL_3             (03u)
#define DEBUG_MESSAGE_LEVEL_4             (04u)
#define DEBUG_MESSAGE_LEVEL_5             (05u)
#define DEBUG_MESSAGE_LEVEL_6             (06u)
#define DEBUG_MESSAGE_LEVEL_7             (07u)

#ifdef DEBUG_PRINT
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
*****************************************************************************/
extern void Debug_Print_Start(uint8 debug_level);

/*****************************************************************************
* Function Name: Debug_Print()
******************************************************************************
* Summary:
* This function is used to print the logs on the UART
*
* Parameters:
* uint8 level - Level of the debug message
* char8 *pszFmt - Message to be print
*
* Return:
* None
*
* Note:
* The message will be printed if the level specified is above the debug level 
* set during the Debug_Start()
*
*****************************************************************************/
extern void Debug_Print(uint8 level, char8 *pszFmt,...);

#else
#define Debug_Print_Start(x)
#define Debug_Print(x,pszFmt,...)
#endif /* DEBUG_PRINT */

#endif /* _DEBUG_H_ */
