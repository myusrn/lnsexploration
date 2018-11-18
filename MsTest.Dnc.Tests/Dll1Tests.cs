using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;

namespace MsTest.Dnc.Tests
{
    [TestClass]
    public class Dll1Tests
    {
        #region dll imports 
        //const string dllName = Environment.Is64BitProcess ? @"..\..\..\..\Dll1\bin\Win32\Debug\Dll1.dll" : @"..\..\..\..\Dll1\bin\x64\Debug\Dll1.dll";
        //const string dllName = @"..\..\..\..\Dll1\bin\Win32\Debug\Dll1.dll";
        const string dllName = @"..\..\..\..\Dll1\bin\x64\Debug\Dll1.dll"; // if you prefer using non-relative paths wrap with System.IO.Path.GetFullPath(dllName)
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Add(double a, double b);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Subtract(double a, double b);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Multiply(double a, double b);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Divide(double a, double b);
        #endregion

        [TestMethod]
        public void Add_SimpleValues_Calculated()
        {
            bool? is64bitprocess = null;
            if (Environment.Is64BitProcess) is64bitprocess = true; // dnc dotnet.exe process is always 64bit even though AnyCPU output is W32i not W32x64 format
            else is64bitprocess = false;

            var expected = 7;
            var actual = Dll1Tests.Add(3, 4);
            //var dll2mathutils = new Dll2.Class1();
            //var actual = dll2mathutils.Add(3, 4);
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow(7, 4, 3)]
        [DataRow(9, 5, 4)]
        public void Subtract_SimpleValues_Calculated(int num1, int num2, int expected)
        {
            //var actual = num1 - num2;
            //var class1 = new Wrc1.Class1();
            //var actual = class1.Subtract(num1, num2);
            var actual = Dll1Tests.Subtract(num1, num2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Multiple_SimpleValues_Calculated()
        {
            var expected = 20;
            //var actual = 4 * 5;
            //var class1 = new Wrc1.Class1();
            //var actual = class1.Multiply(4, 5);
            var actual = Dll1Tests.Multiply(4, 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Divide_SimpleValues_Calculated()
        {
            var expected = 4;
            //var actual = 20 / 5;
            //var class1 = new Wrc1.Class1();
            //var actual = class1.Divide(20, 5);
            var actual = Dll1Tests.Divide(20, 5);
            Assert.AreEqual(expected, actual);
        }
    }
}
