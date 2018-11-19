#pragma once
using namespace System;

// c++/clr tutorial -> quick c++/cli - learn c++/cli in less than 10 minutes -> 
// to create a managed class all you have to do is prefix the class definition with a protection modifier followed by ref, e.g. public ref class MyClass [ sealed ]
// if you leave out ref keyword and compile with /clr switch the class shows up in c# managed code reference but none of the publics methods show up
// where cli in "c++/cli" means c++ modified for common language infrastructure

// c++/clr tutorial -> .net programming with c++/cli (visual c++) | native and .net interoperability | calling native functions from managed code | 
// using c++ interop (implicit pinvoke) -> https://msdn.microsoft.com/en-us/library/2x8kf7zx.aspx

// Add the ability to load native c++ libraries(dll) https://github.com/Azure/Azure-Functions/issues/68 which shows how to create clr c++ 
// Add the ability to load native c++ libraries (dll) https://github.com/Azure/azure-functions-host/issues/1470 which shows how to create clr c++

// initially you get "Managed Debugging Assistant 'LoaderLock' : 'DLL 'fqdn path to dll' is attempting managed execution inside OS Loader lock. Do not attempt to run managed code
// inside a DllMain or image initialization function since doing so can cause the application to hang.'" with a call to this class in place which can be made to go away using 
// debug | exception settings | managed debugging assistants | loaderloack = checked -> unchecked after which you get the following

//CnslApp.Dnf.Tests execution without a call to this class = The program '[21900] CnslApp.Dnf.Tests.exe' has exited with code 0 (0x0).
//CnslApp.Dnf.Tests execution with a call to this class = The program '[27420] CnslApp.Dnf.Tests.exe' has exited with code -532462766 (0xe0434352).
//MsTest.Dnf.Tests execution without a call to this class = The program '[21956] testhost.x86.exe' has exited with code 0 (0x0).
//MsTest.Dnf.Tests execution with a call to this class = The program '[20804] testhost.x86.exe' has exited with code -532462766 (0xe0434352).

// c++ /clr exited with code -532462766 -> ???

namespace Dll2
{
	public ref class MathUtils sealed
	{
	private:
		// todo: move any methods here that you don't want exposed by class scope DllExport statement

	public:
		MathUtils();
		~MathUtils();

		// Returns a + b
		double Add(double a, double b);

		// Returns a - b
		double Subtract(double a, double b);

		// Returns a * b
		double Multiply(double a, double b);

		// Returns a / b
		double Divide(double a, double b);

		String^ ManagedTypeTest(String^ input);		
	};
}