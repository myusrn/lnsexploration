using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using Xunit;

namespace xUnit.Tests
{
    public enum AuthHeaderType {  None, OAuth, XMsClient }

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

// manage nuget packages | browse | Microsoft.Extensions.Configuration.Json | install
// ConfigurationBuilder().AddJsonFile("appsettings.json").Build() GetSection -> https://stackoverflow.com/questions/31453495/how-to-read-appsettings-values-from-json-file-in-asp-net-core
// configuration builder addjsonfile -> https://stackoverflow.com/questions/27382481/why-visual-studio-tell-me-that-addjsonfile-method-is-not-define-in-configurati
// https://docs.microsoft.com/en-us/dotnet/core/tools/project-json-to-csproj
// https://docs.microsoft.com/en-us/nuget/reference/target-frameworks#supported-frameworks

// add | new item | web | json file = appsettings.json
//{ "DoglegCalculatorBackEndUnitTests": { "BackEndFuncAppEndpoint": "https://localhost:7071/api/Calculate", "BackEndWebAppEndpoint": "https://localhost:44301/api/Calculate",
//    "BackEndVmIisAppEndpoint": "https://localhost:44302/api/Calculate"  } }
// thisproject | edit thisproject.csproj | add following to include copying appsettings.json to output path
//<Target Name="CopyToBin" BeforeTargets="Build"> <!-- for build and rebuild inclusion of dllimport referenced dll and appsettings -->
//  <Copy SourceFiles="$(ProjectDir)appsettings.json" DestinationFolder="$(OutputPath)" />
//</Target>

// "xunit theory data driven tests" -> 07nov17 https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
// and 14apr12 https://stackoverflow.com/questions/22093843/pass-complex-parameters-to-theory and 
         
        [Theory, ClassData(typeof(WebApiIntgTestsData))]
        public void SomeMethod_Calculate_UsingClassData_ShouldNotThrowException(string endpoint, AuthHeaderType authHeaderType, string authHeaderValue)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            Assert.NotNull(endpoint);
            Assert.IsType<AuthHeaderType>(authHeaderType);
            Assert.NotNull(authHeaderValue);
        }

        [Theory, MemberData(nameof(WebApiIntgTestsDataMethod))]
        public void SomeMethod_Calculate_UsingMethodData_ShouldNotThrowException(string endpoint, AuthHeaderType authHeaderType, string authHeaderValue)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            Assert.NotNull(endpoint);
            Assert.IsType<AuthHeaderType>(authHeaderType);
            Assert.NotNull(authHeaderValue);
        }

        public static IEnumerable<object[]> WebApiIntgTestsDataMethod()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            
            List<object[]> data = new List<object[]>();
            foreach (var child in config.GetSection("DoglegBackEndIntgTests").GetChildren())
            {
// *** number of items and their types have to match [Theory, ClassData(typeof(WebApiIntgTestsData))] input parameters otherwise things don't work and not obvious why ***
                var authHeaderType = (AuthHeaderType)Enum.Parse(typeof(AuthHeaderType), child["AuthHeaderType"].Replace("AuthHeaderType.", ""));
                data.Add(new object[] { child["Endpoint"], authHeaderType, child["AuthHeaderValue"] });
            }
            return data;
        }

        [Fact]
        public void Add_SimpleValues_Calculated()
        {
            bool? is64bitprocess = null;
            if (Environment.Is64BitProcess) is64bitprocess = true; // dnc dotnet.exe process is always 64bit even though AnyCPU output is W32i not W32x64 format
            else is64bitprocess = false;

            var expected = 7;
            var actual = Dll1Tests.Add(3, 4);
            //var dll2mathutils = new Dll2.MathUtils();
            //var actual = dll2mathutils.Add(3, 4);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(7, 4, 3)]
        [InlineData(9, 5, 4)]
        public void Subtract_SimpleValues_Calculated(int num1, int num2, int expected)
        {
            //var actual = num1 - num2;
            //var class1 = new Wrc1.Class1();
            //var actual = class1.Subtract(num1, num2);
            var actual = Dll1Tests.Subtract(num1, num2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiple_SimpleValues_Calculated()
        {
            var expected = 20;
            //var actual = 4 * 5;
            //var class1 = new Wrc1.Class1();
            //var actual = class1.Multiply(4, 5);
            var actual = Dll1Tests.Multiply(4, 5);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Divide_SimpleValues_Calculated()
        {
            var expected = 4;
            //var actual = 20 / 5;
            //var class1 = new Wrc1.Class1();
            //var actual = class1.Divide(20, 5);
            var actual = Dll1Tests.Divide(20, 5);
            Assert.Equal(expected, actual);
        }
    }
}
