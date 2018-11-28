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
        private static string ClientId = "8e98f706-dc42-4f97-bef2-b51e3e146e06";
        private static string TenantId = "92e34e2f-c298-4006-8b22-1c25f94c2456"; // == mymsdn.onmicrosoft.com
        // Suggested Redirect URIs for public clients (mobile, desktop) | urn:ietf:wg:oauth:2.0:oob = checked
        //public static PublicClientApplication PublicClientApp = new PublicClientApplication(ClientId); // "https://login.microsoftonline.com/common/");
        public static PublicClientApplication PublicClientApp = new PublicClientApplication(ClientId, $"https://login.microsoftonline.com/{TenantId}/");
    }
}
