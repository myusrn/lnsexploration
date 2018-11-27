using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace AzFuncApp1
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous /* .User */, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            bool? is64bitprocess = null;
            if (Environment.Is64BitProcess) is64bitprocess = true; // dnc dotnet.exe process is always 64bit even though AnyCPU output is W32i not W32x64 format
            else is64bitprocess = false;
            log.LogInformation($"process type is " + (is64bitprocess == true ? "64 bit" : "32 bit"));

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

#region openid/oauth security code
            var isAuthenticated = Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity.IsAuthenticated ? true: false;
            ClaimsIdentity identity = null; bool isInCommodityRole = false, isInProprietaryRole = false;
            Dictionary<string, string> claims = null; List<string> roles = new List<string> { "None" };
            if (!isAuthenticated)
            {
                log.LogInformation("Not authenticated");
            }
            else
            {
                identity = (Thread.CurrentPrincipal as ClaimsPrincipal)?.Identity as ClaimsIdentity;

                log.LogInformation("Current user is authenticated as " + identity.Name);
                foreach (var claim in identity.Claims) log.LogInformation($"claim type = {claim.Type} and value = {claim.Value}");

                if (Thread.CurrentPrincipal.IsInRole("Commodity")) isInCommodityRole = true;
                if (Thread.CurrentPrincipal.IsInRole("Proprietary")) isInProprietaryRole = true;
                log.LogInformation($"Current user Commoditity role = {isInCommodityRole} and Proprietary role = {isInProprietaryRole}");

                claims = identity.Claims.ToDictionary(c => c.Type, c => c.Value);
                foreach (var claim in claims) log.LogInformation($"claim key = {claim.Key} and value = {claim.Value}");

                roles = identity.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();
                foreach (var role in roles) log.LogInformation($"role value = {role}");
            }

            // if input parameters using [Microsoft.AspNetCore.Http.]HttpRequest req -> System.Net.Http.HttpRequestMessage request
            // then could just return claim set as response here
            //return request.CreateResponse(HttpStatusCode.OK, claims, "application/json");
#endregion

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
