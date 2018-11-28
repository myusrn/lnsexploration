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
        private static string ApplicationId = "8e98f706-dc42-4f97-bef2-b51e3e146e06";

        public static PublicClientApplication PublicClientApp = new PublicClientApplication(ApplicationId);
    }
}
