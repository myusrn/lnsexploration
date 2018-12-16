using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AzFuncApp1
{
    public static class Function1
    {
        #region dll imports
//requires <project>.csproj | edit | addition of following
//<ItemGroup>
//  <None Include="$(ProjectDir)..\Dll1\bin\Win32\$(Configuration)\Dll1.dll"> <!-- for pack and publish inclusion of dllimport referenced dll -->
//    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
//  </None>
//</ItemGroup>  
//<Target Name="CopyToBin" BeforeTargets="Build"> <!-- for build and rebuild inclusion of dllimport referenced dll -->
//  <Copy SourceFiles = "$(ProjectDir)..\Dll1\bin\Win32\$(Configuration)\Dll1.dll" DestinationFolder="$(OutputPath)\bin" />
//</Target>
        //const string dllName = Environment.Is64BitProcess ? @"..\..\..\..\Dll1\bin\Win32\Debug\Dll1.dll" : @"..\..\..\..\Dll2\bin\x64\Debug\Dll1.dll";
        //const string dllName = @"..\..\..\..\Dll1\bin\Win32\Debug\Dll1.dll";
        //const string dllName = @"..\..\..\..\Dll1\bin\x64\Debug\Dll1.dll"; // if you prefer using non-relative paths wrap with System.IO.Path.GetFullPath(dllName)
        const string dllName = @"Dll1.dll";  // when using post-build event setting
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Add(double a, double b);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Subtract(double a, double b);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Multiply(double a, double b);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        static extern double Divide(double a, double b);
        #endregion

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ClaimsPrincipal principal)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody); // object(JObject)
            //dynamic data = JsonConvert.DeserializeObject<Dictionary<string, int>>(requestBody); // throws exception

            //name = name ?? data?.name; // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
            // and https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-conditional-operators on the right hand operand

            //name = name ?? data?.name;
            var dataJObj = data as JObject;
            name = name ?? dataJObj["name1"].Value<string>();

            if (name != null) log.LogInformation($"name property passed was \"{name}\"");

#region additions to test dllimport and c++/cli managed assembly reference calls
            bool? is64bitprocess = null;
            if (Environment.Is64BitProcess) is64bitprocess = true; // aka.ms/azAdm | <function app> | application settings | general settings | platform = 32-bit (default) or 64-bit
            else is64bitprocess = false; // localhost %localappdata%\azurefunctionstools\releases\2.11.3\cli\func.exe is W32i / 32bit process
            log.LogInformation($"process type is " + (is64bitprocess == true ? "64 bit" : "32 bit"));

            log.LogInformation($"just before dll1mathutilsAddTest");
            //var dll1mathutilsAddTest = 7; // dummy placeholder value
            var dll1mathutilsAddTest = Add(4, 3); // c++ native code dllexport/import
            //var dll2mathutilsAddTest = new Dll2().Add(4, 3); // c++ native code /clr output reference
            log.LogInformation($"just after dll1mathutilsAddTest");
#endregion

            return name != null // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator, aka ternary conditional operator
                ? (ActionResult)new OkObjectResult($"Hello, {name} from updated release where dll1mathutilsAddTest generated {dll1mathutilsAddTest}!")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
