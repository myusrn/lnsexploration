using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace xUnit.Tests
{
    public enum AuthHeaderType
    {
        EasyAuth,
        OAuth,
        None
    }

    public class WebApiIntgTestsData : IEnumerable<object[]>
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
}
