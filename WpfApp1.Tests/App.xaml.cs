using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.Tests
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // aad app registrations | <your application> | overview | application (aka client) id
        // Suggested Redirect URIs for public clients (mobile, desktop) | urn:ietf:wg:oauth:2.0:oob = checked

        static string clientId = ConfigurationManager.AppSettings.Get("ida:ClientId");
        static string tenantId = ConfigurationManager.AppSettings.Get("ida:TenantId");
        static string signinBaseAddress = ConfigurationManager.AppSettings.Get("ida:SigninBaseAddress");

        public static PublicClientApplication PublicClientApp = new PublicClientApplication(clientId, $"{signinBaseAddress}/{tenantId}/");  // tenant only wsa
        //public static PublicClientApplication PublicClientApp = new PublicClientApplication(clientId), $"{signinBaseAddress}/organizations/");  // any tenant wsa
        //public static PublicClientApplication PublicClientApp = new PublicClientApplication(clientId), $"{signinBaseAddress}/consumers/");  // only msa
        //public static PublicClientApplication PublicClientApp = new PublicClientApplication(clientId), $"{signinBaseAddress}/common/");  // any wsa or msa
    }
}
