using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;

namespace MsTest.Dnc.Tests
{
    [TestClass]
    public class Dll1Tests
    {
        #region dll imports 
        //const string dllName = Environment.Is64BitProcess ? @"d:\temp\WrapTest\Dll1\bin\Debug\Win32\Dll1.dll" : @"d:\temp\WrapTest\Dll1\bin\Debug\x64\Dll1.dll";
        //const string dllName = @"..\..\..\..\Dll1\bin\Debug\Win32\Dll1.dll";
        const string dllName = @"..\..\..\..\Dll1\bin\Debug\x64\Dll1.dll"; // if you prefer using non-relative paths wrap with System.IO.Path.GetFullPath(dllName)()
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
            if (Environment.Is64BitProcess) is64bitprocess = true; // dnc test runner is 64bit process even when test | settings | processor architecture = x86
            else is64bitprocess = false;

            var expected = 7;
            var actual = Dll1Tests.Add(3, 4);
            //var class1 = new Dll3.Class1();
            //var actual = class1.Add(3, 4);
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
