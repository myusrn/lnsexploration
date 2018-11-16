using System;
using System.Runtime.InteropServices;

namespace CnslApp.Dnc.Tests
{
    class Program
    {
        #region dll imports 
        //const string dllNameWithPath = Environment.Is64BitProcess ? @"d:\temp\WrapTest\Dll1\bin\Debug\Win32\Dll1.dll" : @"d:\temp\WrapTest\Dll1\bin\Debug\x64\Dll1.dll";
        //const string dllNameWithPath = @"..\..\..\..\Dll1\bin\Debug\Win32\Dll1.dll";
        const string dllNameWithPath = @"..\..\..\..\Dll1\bin\Debug\x64\Dll1.dll"; // if you prefer using non-relative paths wrap with System.IO.Path.GetFullPath()
        [DllImport(dllNameWithPath, CallingConvention = CallingConvention.Cdecl)]
        static extern double Add(double a, double b);
        [DllImport(dllNameWithPath, CallingConvention = CallingConvention.Cdecl)]
        static extern double Subtract(double a, double b);
        [DllImport(dllNameWithPath, CallingConvention = CallingConvention.Cdecl)]
        static extern double Multiply(double a, double b);
        [DllImport(dllNameWithPath, CallingConvention = CallingConvention.Cdecl)]
        static extern double Divide(double a, double b);
        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            bool? is64bitprocess = null;
            if (Environment.Is64BitProcess) is64bitprocess = true; // dnc test runner is 64bit process even when test | settings | processor architecture = x86 
            else is64bitprocess = false;

            var expected = 7;
            var actual = Program.Add(3, 4);
            Console.WriteLine($"expected = {expected} and actual = {actual}");
        }
    }
}
