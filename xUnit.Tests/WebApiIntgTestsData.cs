using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace xUnit.Tests
{
    public class WebApiIntgTestsData : IEnumerable<object[]>
    {
        //static readonly IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        //string backEndFuncAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndFuncAppEndpoint"];
        //string backEndWebAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndWebAppEndpoint"];
        //string backEndVmIisAppEndpoint = config["DoglegCalculatorBackEndUnitTests:BackEndVmIisAppEndpoint"];

        //static readonly IEnumerable<IConfigurationSection> doglegCalculatorBackEndIntgTests = config.GetSection("DoglegBackEndIntgTests").GetChildren();

        //private readonly List<object[]> data = new List<object[]>()
        //{
        //    new object[] { "ep1", AuthHeaderType.XMsClient, "ahv1" },
        //    new object[] { "ep2", AuthHeaderType.OAuth, "ahv2" },
        //    new object[] { "ep3", AuthHeaderType.None, "ahv3" }
        //};
        //private readonly List<object[]> data = new List<object[]>();

        //public WebApiIntgTestsData()
        //{
        //    var tsec1 = config.GetSection("TestSection1").Value;
        //    var tsec2 = config.GetSection("TestSection2")["Name"];
        //    var tsec2b = config["TestSection2:Name"];
        //    var tsec3 = config.GetSection("TestSection3").GetChildren();
        //    var test3b = config.GetSection("TestSection3").GetChildren().ToString();

        //    var tchld1 = config.GetChildren();
        //    foreach (var child in tchld1) { var tchld = $"key={child.Key} name={child["Name"]} endpoint={child["Endpoint"]}"; }
        //    var tchld2 = config.GetSection("TestSection3").GetChildren();
        //    foreach (var child in tchld2) { var tchld = $"key={child.Key} name={child["Name"]} endpoint={child["Endpoint"]}"; }

        //    var config = new ConfigurationBuilder()
        //        //.SetBasePath(env.ContentRootPath) // adnc
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true) // adnc, for example appsettings.Development.json
        //        //.AddUserSecrets() // adnc, see http://go.microsoft.com/fwlink/?LinkID=532709 -> https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets
        //        //.AddEnvironmentVariables() // adnc
        //        .Build();

        //    //data.Add(new object[] { "ep1", AuthHeaderType.XMsClient, "ahv1" });
        //    //data.Add(new object[] { "ep2", AuthHeaderType.OAuth, "ahv2" });
        //    //data.Add(new object[] { "ep3", AuthHeaderType.None, "ahv3" });

        //    //string[] dataSuffixes = new string[] { "1", "2", "3" };
        //    //foreach (var dataSuffix in dataSuffixes)
        //    //{
        //    //    data.Add(new object[] { $"ep{dataSuffix}", AuthHeaderType.None, $"ahv{dataSuffix}" });
        //    //}

        //    foreach (var child in config.GetSection("DoglegBackEndIntgTests").GetChildren())
        //    {
//// *** number of items and their types have to match [Theory, ClassData(typeof(WebApiIntgTestsData))] input parameters otherwise things don't work and not obvious why ***
        //        var authHeaderType = (AuthHeaderType)Enum.Parse(typeof(AuthHeaderType), child["AuthHeaderType"].Replace("AuthHeaderType.", "")); 
        //        data.Add(new object[] { child["Endpoint"], authHeaderType, child["AuthHeaderValue"] });
        //    }
        //}

        //public IEnumerator<object[]> GetEnumerator() { return data.GetEnumerator(); }
        //IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public IEnumerator<object[]> GetEnumerator()
        {
            //yield return new object[] { "ep1", AuthHeaderType.XMsClient, "ahv1" };
            //yield return new object[] { "ep2", AuthHeaderType.OAuth, "ahv2" };
            //yield return new object[] { "ep3", AuthHeaderType.None, "ahv3" };

            var config = new ConfigurationBuilder()
                //.SetBasePath(env.ContentRootPath) // adnc
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true) // adnc, for example appsettings.Development.json
                //.AddUserSecrets() // adnc, see http://go.microsoft.com/fwlink/?LinkID=532709 -> https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets
                //.AddEnvironmentVariables() // adnc
                .Build();

//            List<object[]> data = new List<object[]>();
//            foreach (var child in config.GetSection("DoglegBackEndIntgTests").GetChildren())
//            {
//// *** number of items and their types have to match [Theory, ClassData(typeof(WebApiIntgTestsData))] input parameters otherwise things don't work and not obvious why ***
//                var authHeaderType = (AuthHeaderType)Enum.Parse(typeof(AuthHeaderType), child["AuthHeaderType"].Replace("AuthHeaderType.", ""));
//                data.Add(new object[] { child["Endpoint"], authHeaderType, child["AuthHeaderValue"] });
//            }
//            return data.GetEnumerator();

            foreach (var child in config.GetSection("DoglegBackEndIntgTests").GetChildren())
            {
// *** number of items and their types have to match [Theory, ClassData(typeof(WebApiIntgTestsData))] input parameters otherwise things don't work and not obvious why ***
                var authHeaderType = (AuthHeaderType)Enum.Parse(typeof(AuthHeaderType), child["AuthHeaderType"].Replace("AuthHeaderType.", ""));
                yield return new object[] { child["Endpoint"], authHeaderType, child["AuthHeaderValue"] };
            }
        }

        //IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
