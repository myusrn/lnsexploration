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
using System.Collections.Generic;
using System.Linq;

namespace AzFuncApp1
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ClaimsPrincipal principal)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            bool? is64bitprocess = null;
            if (Environment.Is64BitProcess) is64bitprocess = true; // aka.ms/azAdm | <function app> | application settings | general settings | platform = 32-bit (default) or 64-bit
            else is64bitprocess = false; // localhost %localappdata%\azurefunctionstools\releases\2.11.3\cli\func.exe is W32i / 32bit process
            log.LogInformation($"process type is " + (is64bitprocess == true ? "64 bit" : "32 bit"));

#region openid/oauth security code
            var idsCount = principal.Identities.Count();
            log.LogInformation($"principal.Identities.Count() = {idsCount}");

            var idIsAuthenticated = principal.Identity.IsAuthenticated; var idName = principal.Identity.Name;
            log.LogInformation($"principal.Identity.IsAuthenticated = '{idIsAuthenticated}' and principal.Identity.Name = '{idName}'");

            var idfIsAuthenticated = principal.Identities.First().IsAuthenticated; var idlIsAuthenticated = principal.Identities.Last().IsAuthenticated;
            log.LogInformation($"principal.Identities.First().IsAuthenticated = {idfIsAuthenticated}, principal.Identities.Last().IsAuthenticated = {idlIsAuthenticated}");

            var idfName = principal.Identities.First().Name; var idlName = principal.Identities.Last().Name;
            log.LogInformation($"principal.Identities.First().Name = '{idfName}', principal.Identities.Last().Name = '{idlName}'");

            //var owner = (principal.FindFirst(ClaimTypes.NameIdentifier))?.Value;
            string[] identityStrings = principal.Identities.Select(GetIdentityString).ToArray();
           
            bool isInCommodityRole = false, isInProprietaryRole = false;
            Dictionary<string, string> claims = null; List<string> roles = new List<string> { "None" };
            if (!idIsAuthenticated)
            {
                log.LogInformation("Not authenticated");
            }
            else
            {
                log.LogInformation("Current user is authenticated as " + principal.Identity.Name);
                foreach (var claim in principal.Claims) log.LogInformation($"claim type = {claim.Type} and value = {claim.Value}");

                if (principal.IsInRole("Commodity")) isInCommodityRole = true;
                if (principal.IsInRole("Proprietary")) isInProprietaryRole = true;
                log.LogInformation($"Current user in Commoditity role = {isInCommodityRole} and in Proprietary role = {isInProprietaryRole}");

                claims = principal.Claims.ToDictionary(c => c.Type, c => c.Value);
                foreach (var claim in claims) log.LogInformation($"claim key = {claim.Key} and value = {claim.Value}");

                roles = principal.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();
                foreach (var role in roles) log.LogInformation($"role value = {role}");
            }
#endregion

            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");

            //return new OkObjectResult($"principal.Identities.Count() = {idsCount}, principal.Identity.IsAuthenticated = '{idIsAuthenticated}' and principal.Identity.Name = '{idName}'");
            return new OkObjectResult(string.Join(";", identityStrings));
        }

        private static string GetIdentityString(ClaimsIdentity identity)
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
    }
}
