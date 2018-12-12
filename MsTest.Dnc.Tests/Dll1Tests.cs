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

// ConfigurationBuilder().AddJsonFile("appsettings.json").Build() GetSection -> https://stackoverflow.com/questions/31453495/how-to-read-appsettings-values-from-json-file-in-asp-net-core
// configuration builder addjsonfile -> https://stackoverflow.com/questions/27382481/why-visual-studio-tell-me-that-addjsonfile-method-is-not-define-in-configurati
// https://docs.microsoft.com/en-us/dotnet/core/tools/project-json-to-csproj
// https://docs.microsoft.com/en-us/nuget/reference/target-frameworks#supported-frameworks

// add | new item | general | text file = App.config
//<?xml version="1.0" encoding="utf-8" ?>
//<configuration>
//  <appSettings>
//    <add key="BackEndFuncAppEndpoint" value="https://localhost:7071/api/Calculate" />
//    <add key="BackEndWebAppEndpoint" value="https://localhost:44301/api/Calculate" />
//    <add key="BackEndVmIisAppEndpoint" value="https://localhost:44302/api/Calculate" />
//  </appSettings>
//</configuration>

//        //static ExeConfigurationFileMap map = new ExeConfigurationFileMap($"{Assembly.GetExecutingAssembly().GetName().Name}.dll.config");
//        //static Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
//        //const string backEndFuncAppEndpoint = config.AppSettings.Settings["BackEndFuncAppEndpoint"].Value;
//        //const string backEndWebAppEndpoint = config.AppSettings.Settings["BackEndWebAppEndpoint"].Value;
//        //const string backEndVmIisWebEndpoint = config.AppSettings.Settings["BackEndVmIisAppEndpoint"].Value;

//        //static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
//        //const string backEndFuncAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndFuncAppEndpoint"];
//        //const string backEndWebAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndWebAppEndpoint"];
//        //const string backEndFuncAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndVmIisAppEndpoint"];

//        static string backEndFuncAppEndpoint, backEndWebAppEndpoint, backEndVmIisWebEndpoint;

//        [ClassInitialize]
//        public static void ClassInit(TestContext context)
//        {
//// mstest test initialize attributes ->  [AssemblyInitialize] static, [ClassInitialize], [TestInitialize], [TestCleanup], [ClassCleanup] static, [AssemblyCleanup] static
//            //var backendEndpoint = ConfigurationManager.AppSettings["BackEndFuncAppEndpoint"]; // returns null
//// mstest .net core configurationmanager.appsettings empty -> https://stackoverflow.com/questions/50529866/reading-from-app-config-in-net-core-application-referencing-net-standard-libra
//// requires <mstest .net core test project> | manage nuget packages | browse System.Configuration.ConfigurationManager | install 
//// mstest .net core appsettings.json vs app.config settings -> https://www.jerriepelser.com/blog/using-configuration-files-in-dotnet-core-unit-tests/ and
//// https://stackoverflow.com/questions/38171748/using-and-injecting-from-appsettings-json-in-unit-tests-on-net-core
//// so better story is to use .net core project typical appsettings.json and appsettings.Development.json overrides
///
//            var map = new ExeConfigurationFileMap();
//            //map.ExeConfigFilename = "testhost.dll.config";
//            //var executingAssembly1 = Assembly.GetExecutingAssembly().GetName().Name;
//            //var executingAssembly2 = typeof(DoglegCalculatorBackEndUnitTests).Assembly.GetName().Name;
//            map.ExeConfigFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.dll.config";
//            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

//            backEndFuncAppEndpoint = config.AppSettings.Settings["BackEndFuncAppEndpoint"].Value;
//            backEndWebAppEndpoint = config.AppSettings.Settings["BackEndWebAppEndpoint"].Value;
//            backEndVmIisWebEndpoint = config.AppSettings.Settings["BackEndVmIisAppEndpoint"].Value;

// configurationbuilder().addjsonfile("appsettings.json").build() getsection -> https://stackoverflow.com/questions/31453495/how-to-read-appsettings-values-from-json-file-in-asp-net-core
//            var config = new ConfigurationBuilder()
//                //.SetBasePath(env.ContentRootPath) // adnc
//                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true) // adnc, for example appsettings.Development.json
//                //.AddUserSecrets() // adnc, see http://go.microsoft.com/fwlink/?LinkID=532709 -> https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets
//                //.AddEnvironmentVariables() // adnc
//                .Build();

//            var test1 = config.GetSection("DoglegCalculatorBackEndUnitTests:BackEndFuncAppEndpoint").Value;
//            var test2 = config["DoglegCalculatorBackEndUnitTests:BackEndFuncAppEndpoint"];
//            var dcbutSection = config.GetSection("DoglegCalculatorBackEndUnitTests");
//            backEndFuncAppEndpoint = dcbutSection["BackEndFuncAppEndpoint"];
//            backEndWebAppEndpoint = dcbutSection["BackEndWebAppEndpoint"];
//            backEndVmIisWebEndpoint = dcbutSection["BackEndVmIisAppEndpoint"];
//        }

//        [DataTestMethod]
//        //[DataRow(backEndFuncAppEndpoint)]
//        [DataRow("https://localhost:7071/api/calculate")]
//        [DataRow("https://localhost:44301/api/calculate")]
//        [DataRow("https://localhost:44302/api/calculate")]
//        public void SomeMethod_Calculate_ShouldNotThrowException(string backendEndpoint)
//        {
//            // TODO: use DataTestMethod/DataRow input parameter assignment in test
//        }

        [TestMethod]
        public void Add_SimpleValues_Calculated()
        {
            bool? is64bitprocess = null;
            if (Environment.Is64BitProcess) is64bitprocess = true; // dnc dotnet.exe process is always 64bit even though AnyCPU output is W32i not W32x64 format
            else is64bitprocess = false;

            var expected = 7;
            var actual = Dll1Tests.Add(3, 4);
            //var dll2mathutils = new Dll2.MathUtils();
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
