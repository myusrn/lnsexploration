using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace xUnit.Tests
{
    public class AzFuncApp1WebApp1UnitTests : FunctionControllerUnitTest
    {
        // c# mock claimsprincipal -> https://stackoverflow.com/questions/38323895/how-to-add-claims-in-a-mock-claimsprincipal
        // and https://stackoverflow.com/questions/162534/mock-iidentity-and-iprincipal

        [Theory, InlineData("FuncApp"), InlineData("WebApp")]
        public async Task WebApi_FunctionController_ShouldNotThrowExceptionAsync(string backendController)
        {
            bool? is64bitprocess = null;
            if (Environment.Is64BitProcess) is64bitprocess = true; // aka.ms/azAdm | <function app> | application settings | general settings | platform = 32-bit (default) or 64-bit
            else is64bitprocess = false; // localhost %localappdata%\azurefunctionstools\releases\2.11.3\cli\func.exe is W32i / 32bit process
            Debug.WriteLine($"process type is " + (is64bitprocess == true ? "64 bit" : "32 bit"));

            var result = string.Empty;

            switch (backendController)
            {
                case "FuncApp":
                    //var query = new Dictionary<String, StringValues>(); query.TryAdd("name", "myusrn");
                    //var body = JsonConvert.SerializeObject("{ 'name': 'myusrn' }");
                    //var body = JsonConvert.SerializeObject("{ \"name\": \"myusrn\" }");
                    var body = JsonConvert.SerializeObject(new object[] { "name", "myusrn" });
                    var objectResult = await AzFuncApp1.Function1.Run(req: HttpRequestSetup(/* query */ null, body), principal: principal, log: log);
                    result = (objectResult as OkObjectResult).Value as string;
                    break;
                case "WebApp":
                    var controller = new AzWebApp1.Controllers.ValuesController(Mock.Of<ILogger<AzWebApp1.Controllers.ValuesController>>());
                    MockHttpContext(controller);
                    //result = controller.Post();
                    result = (controller.Post("{ 'name': 'myusrn'}") as OkObjectResult).Value as string;
                    break;
            }
           
            Assert.Equal("myusrn", result);
        }
    }
}
