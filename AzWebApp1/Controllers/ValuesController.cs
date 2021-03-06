﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
//using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzWebApp1.Controllers
{
    //[Authorize]
    //[Authorize(Roles = "Admin, Writer, Approver")]
    [Authorize(Roles = "Commodity, Proprietary")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        #region dll imports
//requires <project>.csproj | edit | addition of following
//<Target Name="CopyToBin" BeforeTargets="Build"> <!-- for build and rebuild inclusion of dllimport referenced dll -->
//  <Copy SourceFiles="$(ProjectDir)..\Dll1\bin\Win32\$(Configuration)\Dll1.dll" DestinationFolder="$(OutputPath)" />
//  <!--<Copy SourceFiles="$(ProjectDir)..\Dll1\bin\x64\$(Configuration)\Dll1.dll" DestinationFolder="$(OutputPath)" />-->
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

        ILogger<ValuesController> log;

        public ValuesController(ILogger<ValuesController> log)
        {
            this.log = log;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(/* [FromServices]IHttpClientFactory factory */)
        {
            log.LogInformation("C# HTTP get trigger controller processed a request.");

            //var client = new HttpClient();
            //var client = factory.CreateClient("api"); // create httpclient with Startup.cs ConfigureServices defined policy behaviors

            //var UserWebApi = System.Threading.Thread.CurrentPrincipal as System.Security.Claims.ClaimsPrincipal;
            //var UserWebApp = System.Web.Mvc.Controller.User as System.Security.Claims.ClaimsPrincipal;
            var identity = User.Identity;
            var claims = User.Claims;
            var hasAdminRoleClaim = User.HasClaim("role", "Admin");
            var isInAdminRole = User.IsInRole("Admin");

            var isEasyAuth = this.Request.Headers.ContainsKey("X-MS-CLIENT-PRINCIPAL-ID");
            var isAuthenticated = User.Identity.IsAuthenticated;
            var idName = string.IsNullOrEmpty(User.Identity.Name) ? "null" : User.Identity.Name;
            string owner = (User.FindFirst(ClaimTypes.NameIdentifier))?.Value;
            log.LogInformation($"isAuthenticated = {isAuthenticated}, idName = {idName}, owner = {owner}");

            bool isInCommodityRole = User.IsInRole("Commodity"), isInProprietaryRole = User.IsInRole("Proprietary");
            //isInCommodityRole = User.HasClaim(ClaimTypes.Role, "Commodity"); isInProprietaryRole = User.HasClaim(ClaimTypes.Role, "Proprietary");
            log.LogInformation($"Current user in Commoditity role = {isInCommodityRole} and in Proprietary role = {isInProprietaryRole}");

            string[] identityStrings = User.Identities.Select(GetIdentityString).ToArray();
            log.LogInformation($"identityStrings = {string.Join(";", identityStrings)}");

            var result = new List<String>(identityStrings);
            result.Add($"isAuthenticated: {isAuthenticated}");
            var headers = this.Request.Headers; foreach (var header in headers) result.Add($"{header.Key}: {header.Value}");

            //return new string[] { "value1", "value2" };
            return result;
        }

        string GetIdentityString(ClaimsIdentity identity)
        {
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                // user identity
                var userNameClaim = identity.FindFirst(ClaimTypes.Name);
                return $"Identity: ({identity.AuthenticationType}, {userNameClaim?.Value}, {userIdClaim?.Value})";
            }
            else
            {
                // key based identity
                var authLevelClaim = identity.FindFirst("http://schemas.microsoft.com/2017/07/functions/claims/authlevel");
                var keyIdClaim = identity.FindFirst("http://schemas.microsoft.com/2017/07/functions/claims/keyid");
                return $"Identity: ({identity.AuthenticationType}, {authLevelClaim?.Value}, {keyIdClaim?.Value})";
            }
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //    // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        //}

        // POST api/values
        [HttpPost]
        //public DoglegClassifications Post([FromBody] double measuredDepth, double doglegSeverity)
        public IActionResult Post([FromBody] string body)
        {
            log.LogInformation("C# HTTP post trigger controller processed a request.");

            dynamic data = JsonConvert.DeserializeObject(body);
            var name = JObject.Parse(body)["name"].Value<string>();

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

            //return name;
            //return (ActionResult)new OkObjectResult(new string[] { "value1", "value2" });
            return name != null // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator, aka ternary conditional operator
                ? (ActionResult)new OkObjectResult($"Hello, {name} from updated release where dll1mathutilsAddTest generated {dll1mathutilsAddTest}!")
                : new BadRequestObjectResult("Please pass a name [ on the query string or ] in the request body");
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //    // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        //}
    }
}
