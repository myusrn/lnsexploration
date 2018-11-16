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
        [DllImport(@"d:\temp\WrapTest\Debug\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double Add(double a, double b);
        [DllImport(@"d:\temp\WrapTest\Debug\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double Subtract(double a, double b);
        [DllImport(@"d:\temp\WrapTest\Debug\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double Multiply(double a, double b);
        [DllImport(@"d:\temp\WrapTest\Debug\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double Divide(double a, double b);
        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            bool? is64bitprocess = null;
            if (Environment.Is64BitProcess) is64bitprocess = true;  // dnc test runner is 64bit process even when test | settings | processor architecture = x86 ??? 
            else is64bitprocess = false;

            var expected = 7; var actual = Program.Add(3, 4);
            Console.WriteLine($"expected = {expected} and actual = {actual}");
        }
    }
}
