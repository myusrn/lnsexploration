using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzWebApp1.Controllers
{
    //[Authorize]
    //[Authorize(Roles = "Admin, Writer, Approver")]
    [Authorize(Roles = "Commodity, Proprietary")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        ILogger<ValuesController> log;

        public ValuesController(ILogger<ValuesController> logger)
        {
            this.log = logger;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(/* [FromServices]IHttpClientFactory factory */)
        {
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

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }
    }
}
