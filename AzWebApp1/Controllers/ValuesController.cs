using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [Authorize]
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

            var isAuthenticated = User.Identity.IsAuthenticated;
            var idName = string.IsNullOrEmpty(User.Identity.Name) ? "null" : User.Identity.Name;
            string owner = (User.FindFirst(ClaimTypes.NameIdentifier))?.Value;
            //string[] identityStrings = User.Identities.Select(GetIdentityString).ToArray();

            return new string[] { "value1", "value2" };
        }

        string GetIdentityString(ClaimsIdentity identity)
        {
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                // user identity
                var userNameClaim = identity.FindFirst(ClaimTypes.Name);
                return $"Identity: ({identity.AuthenticationType}, {userNameClaim.Value}, {userIdClaim.Value})";
            }
            else
            {
                // key based identity
                var authLevelClaim = identity.FindFirst("http://schemas.microsoft.com/2017/07/functions/claims/authlevel");
                var keyIdClaim = identity.FindFirst("http://schemas.microsoft.com/2017/07/functions/claims/keyid");
                return $"Identity: ({identity.AuthenticationType}, {authLevelClaim.Value}, {keyIdClaim.Value})";
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
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
