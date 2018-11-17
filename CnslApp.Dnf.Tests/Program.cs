using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CnslApp.Dnf.Tests
{
    class Program
    {
        #region dll imports 
        //const string dllName = Environment.Is64BitProcess ? @"..\..\..\Dll1\bin\Win32\Debug\Dll1.dll" : @"..\..\..\Dll1\bin\x64\Debug\Dll1.dll";
        const string dllName = @"..\..\..\Dll1\bin\Win32\Debug\Dll1.dll";
        //const string dllName = @"..\..\..\Dll1\bin\x64\Debug\Dll1.dll"; // if you prefer using non-relative paths wrap with System.IO.Path.GetFullPath(dllName)
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Add(double a, double b);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Subtract(double a, double b);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Multiply(double a, double b);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Divide(double a, double b);
        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            bool? is64bitprocess = null;
            if (Environment.Is64BitProcess) is64bitprocess = true; // dnc dotnet.exe process is always 64bit even though AnyCPU output is W32i not W32x64 format
            else is64bitprocess = false;

            var expected = 7;
            //var actual = Program.Add(3, 4); // platform invoke c# [DllImport] / c++ dll extern "C" __declspec( dllexport )  
            var dll2mathutils = new Dll2.MathUtils();
            var actual = dll2mathutils.Add(3, 4); // c++ dll /clr generated managed code build output that is directly referencable 
            Console.WriteLine($"expected = {expected} and actual = {actual}");
        }
    }
}
