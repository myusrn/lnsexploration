using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using Xunit;

namespace xUnit.Tests
{
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

        public enum AuthHeaderType
        {
            EasyAuth,
            OAuth,
            None
        }
            
        [Theory]
        [ClassData(typeof(WebApiTestData))]
        public void SomeMethod_Calculate_ShouldNotThrowException(string backendEndpoint, AuthHeaderType authHeaderType, string authHeaderValue)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, backendEndpoint);

            Assert.NotNull(backendEndpoint);
            Assert.IsType<AuthHeaderType>(authHeaderType);
            Assert.NotNull(authHeaderValue);
        }

        public class WebApiTestData : IEnumerable<object[]>
        {
            static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            //string backEndFuncAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndFuncAppEndpoint"];
            //string backEndWebAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndWebAppEndpoint"];
            //string backEndVmIisAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndVmIisAppEndpoint"];

            //public WebApiTestData()
            //{
            //    var config = new ConfigurationBuilder()
            //        //.SetBasePath(env.ContentRootPath) // adnc
            //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //        //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true) // adnc, for example appsettings.Development.json
            //        //.AddUserSecrets() // adnc, see http://go.microsoft.com/fwlink/?LinkID=532709 -> https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets
            //        //.AddEnvironmentVariables() // adnc
            //        .Build();

            //    backEndFuncAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackFuncWebAppEndpoint"];
            //    backEndWebAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndWebAppEndpoint"];
            //    backEndFuncAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndVmIisAppEndpoint"];
            //}

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { config["DoglegCalculatorBackEndUnitTests:BackEndFuncAppEndpoint"], AuthHeaderType.EasyAuth, string.Empty };
                yield return new object[] { config["DoglegCalculatorBackEndUnitTests:BackEndWebAppEndpoint"], AuthHeaderType.EasyAuth, string.Empty };
                yield return new object[] { config["DoglegCalculatorBackEndUnitTests:BackEndVmIisAppEndpoint"], AuthHeaderType.OAuth, string.Empty };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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
