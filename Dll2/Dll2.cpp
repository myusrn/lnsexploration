// Dll2.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

//#define DllImport   __declspec( dllimport ) 
//#define DllExport   __declspec( dllexport ) 
#define DllExport   extern "C" __declspec( dllexport )  
//#define DllExport   extern "C"

DllExport double DummyDllExportFunctionToCauseLibToBeCreated(double a, double b) 
{
	return a + b;
}
