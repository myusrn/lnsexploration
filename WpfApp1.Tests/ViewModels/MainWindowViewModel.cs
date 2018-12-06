using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1.Tests.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            MsalAcquireTokenEnabled = true; // vs false (default)
            //CallGraphApiVisibility = CallMyWebApiVisibility = Visibility.Visible; // (default)
            GraphApiCallResults = MyWebApiCallResults = TokenInfo = "nothing yet";
            MsalSignOutVisibility = Visibility.Collapsed; // vs Visible (default)
        }

        /// <summary>
        /// A collection of datagrid bindable data
        /// </summary>
        //public ObservableCollection<SomeDataTypeOnlyViewModel> SomeDataGridBindableData { get; set; }

// property setter avoid raisepropertychanged ->
// https://stackoverflow.com/questions/13663719/avoid-calling-raisepropertychanged-in-every-setter ->
// https://archive.codeplex.com/?p=kindofmagic -> https://www.nuget.org/packages/KindOfMagic -> 
// https://github.com/demigor/kindofmagic
// instead of PropertyName { get { return backingvar; } set { if (backingvar != value) { backingvar = value; RaisePropertyChanged("PropertyName"); } }
// requires ViewModel.cs | class MagicAttribute : Attribute { } & class NoMagicAttribute : Attribute { }
// c# alternative to kindofmagic nuget -> nothing
        [Magic]
        public bool MsalAcquireTokenEnabled { get; set; }
        [Magic]
        public Visibility CallGraphApiVisibility { get; set; }
        [Magic]
        public Visibility CallMyWebApiVisibility { get; set; }
        [Magic]
        public Visibility MsalSignOutVisibility { get; set; }
        [Magic]
        public string GraphApiCallResults { get; set; }
        [Magic]
        public string MyWebApiCallResults { get; set; }
        [Magic]
        public string TokenInfo { get; set; }

        private ICommand msalAcquireTokenCommand;
        /// <summary>
        /// Setup the delegates for MsalAcquireToken
        /// </summary>
        public ICommand MsalAcquireTokenCommand
        {
            get
            {
                return msalAcquireTokenCommand ?? (msalAcquireTokenCommand = 
                    new BasicCommand((arg) => MsalAcquireToken(), () => !IsMsalAcquiringToken()));
            }
        }

        private bool isMsalAcquiringToken;
        /// <summary>
        /// Whether or not the msal acquiring token process is in progress
        /// </summary>
        /// <returns></returns>
        public bool IsMsalAcquiringToken()
        {
            return isMsalAcquiringToken;
        }

        public async void MsalAcquireToken()
        {
            isMsalAcquiringToken = true;

            // TODO: execute msal request token code here
            GraphApiCallResults = MyWebApiCallResults = TokenInfo = string.Empty;
            MsalSignOutVisibility = Visibility.Visible;

// scopes = { "user.read" } for graph api call or { "<application id uri>/access_as_user" } for custom web api calls
// where <application id uri> = "api://<application (client) id>" (default) or "https://<azfn>.azurewebsites.net" (azfn express provisioned)
// which surfaces in manifest under "identifierUris": [	"api://<application (client) id>" or "https://<azfn>.azurewebsites.net" ]
// e.g. "identifierUris": [ "api://8e98f706-dc42-4f97-bef2-b51e3e146e06" or "https://azfndn1.azurewebsites.net" or "https://azfndn1ipt.azurewebsites.net" ]
// AADSTS65005: The application 'azfndn1' asked for scope 'access_as_user' that doesn't exist on the resource '<tenant id>'.
// AADSTS700022: The provided value for the input parameter scope is not valid because it contains more than one resource.
// The scope https://azfndn1ipt.azurewebsites.net/user_impersonation offline_access openid profile user.read [ email ] is not valid.
// in case of graph api user.read scope setting the issued token ends up having scp claim = "openid profile User.Read email"
// in case of my web api access_as_user / user_impersonation scope setting the issued token ends up having scp claim = just that value
// scope / scp claim is granular resource.operation.constraint permission versus more course grained user/persona based role permission
// and audience uri / aud claim destination based permission. one is assigned to role directory or via group(s) and scope permission is 
// issued during token acquisition when user consents to delegate that permission to service principal object ???

            var scopes = new string[] { "user.read" }; // graph api setting which ends up being "openid profile User.Read email"
            //var scopes = new string[] { "user.read", "api://8e98f706-dc42-4f97-bef2-b51e3e146e06/user_impersonation" }; // n/a
            //var scopes = new string[] { "openid", "profile", "User.Read", "email", "api://8e98f706-dc42-4f97-bef2-b51e3e146e06/user_impersonation" }; // n/a

            //var scopes = new string[] { "api://8e98f706-dc42-4f97-bef2-b51e3e146e06/access_as_user" }; // quickstart proposed setting using "identifierUris": [ "api://<application (client) id>" ] format
            //var scopes = new string[] { "api://8e98f706-dc42-4f97-bef2-b51e3e146e06/user_impersonation" }; // azfn express provisioned scope using altered "identifierUris": [ "api://<application (client) id>" ] format
            //var scopes = new string[] { "https://azfndn1.azurewebsites.net/user_impersonation" }; // azfndn1 express provisioned scope using default "identifierUris": [ "https://<azfn>.azurewebsites.net" ] format
            //var scopes = new string[] { "https://azfndn1ipt.azurewebsites.net/user_impersonation" }; // azfndn1ipt express provisioned scope using default "identifierUris": [ "https://<azfn>.azurewebsites.net" ] format
            //var scopes = new string[] { "api://8e98f706-dc42-4f97-bef2-b51e3e146e06/Files.Read" }; // app registrations (preview) | <app> | expose an api | add a scope proposed setting using resource.operation[.constraint] format

            AuthenticationResult authResult = await GetAuthResult(scopes, MyWebApiCallResults);

            if (authResult != null)
            {
                DisplayBasicTokenInfo(authResult);
                MsalAcquireTokenEnabled = false;
                MsalSignOutVisibility = Visibility.Visible;
            }

            isMsalAcquiringToken = false;
        }

        private ICommand callGraphApiCommand;
        /// <summary>
        /// Setup the delegates for CallGraphApi
        /// </summary>
        public ICommand CallGraphApiCommand
        {
            get
            {
                return callGraphApiCommand ?? (callGraphApiCommand = 
                    new BasicCommand((arg) => CallGraphApi(), () => !IsCallingGraphApi()));
            }
        }

        private bool isCallingGraphApi;
        /// <summary>
        /// Whether or not the calling graph api process is in progress
        /// </summary>
        /// <returns></returns>
        public bool IsCallingGraphApi()
        {
            return isCallingGraphApi;
        }

        public async void CallGraphApi()
        {
            isCallingGraphApi = true;

            // execute call graph api here
            GraphApiCallResults = TokenInfo = string.Empty;

            var scopes = new string[] { "user.read" }; // graph api setting which ends up being "openid profile User.Read email"

            AuthenticationResult authResult = await GetAuthResult(scopes, GraphApiCallResults);

            if (authResult != null)
            {
                const string graphApiEndpoint = "https://graph.microsoft.com/v1.0/me"; // endpoint expecting one audienceUri in AccessToken
                GraphApiCallResults = await GetHttpContentWithToken(graphApiEndpoint, authResult.AccessToken);
                DisplayBasicTokenInfo(authResult);
                CallGraphApiVisibility = Visibility.Collapsed;
                MsalSignOutVisibility = Visibility.Visible;
            }

            isCallingGraphApi = false;
        }

        private ICommand callMyWebApiCommand;
        /// <summary>
        /// Setup the delegates for CallMyWebApi
        /// </summary>
        public ICommand CallMyWebApiCommand
        {
            get
            {
                return callMyWebApiCommand ?? (callMyWebApiCommand = 
                    new BasicCommand((arg) => CallMyWebApi(), () => !IsCallingMyWebApi()));
            }
        }

        private bool isCallingMyWebApi;
        /// <summary>
        /// Whether or not the calling my web api process is in progress
        /// </summary>
        /// <returns></returns>
        public bool IsCallingMyWebApi()
        {
            return isCallingMyWebApi;
        }

        public async void CallMyWebApi()
        {
            isCallingMyWebApi = true;

            // execute call my web api here
            MyWebApiCallResults = TokenInfo = string.Empty;

            //var scopes = new string[] { "api://8e98f706-dc42-4f97-bef2-b51e3e146e06/access_as_user" }; // quickstart proposed setting using "identifierUris": [ api://<application (client) id> ] format
            var scopes = new string[] { "https://azfndn1.azurewebsites.net/user_impersonation" }; // azfndn1 express provisioned
            //var scopes = new string[] { "https://azfndn1ipt.azurewebsites.net/user_impersonation" }; // azfndn1ipt express provisioned
            //var scopes = new string[] { "api://8e98f706-dc42-4f97-bef2-b51e3e146e06/Files.Read" }; // app registrations (preview) | <app> | expose an api | add a scope proposed setting using resource.operation[.constraint] format
            //var scopes = new string[] { "https://azfndn1ipt.azurewebsites.net/user_impersonation" }; // azfndn1ipt
            //var scopes = new string[] { "api://78ca852a-959d-401e-a025-c665f6696a04/access_as_user" }; // bizlgcsvc

            AuthenticationResult authResult = await GetAuthResult(scopes, MyWebApiCallResults);

            if (authResult != null)
            {
                //const string myWebApiEndpoint = "http://localhost:7071/api/Function1";
                //const string myWebApiEndpoint = "http://localhost:7071/api/Function2";
                //const string myWebApiEndpoint = "https://azfndn1.azurewebsites.net/api/Function1?name=azfndn1%20function1"; // endpoint expecting one audienceUri in AccessToken
                //const string myWebApiEndpoint = "https://azfndn1.azurewebsites.net/api/Function1?code=ZsInfo5UUe1LZHIA9hfGHFqdLifuggnSuq1mP3lbLFlgk9EOZ87pfg==&name=azfndn1%20function1";
                //const string myWebApiEndpoint = "https://azfndn1.azurewebsites.net/api/Function2?name=azfndn1%20function2"; // endpoint expecting one audienceUri in AccessToken
                const string myWebApiEndpoint = "https://azfndn1.azurewebsites.net/api/Function2?code=esuldfz/tgIunGVVfxTLLNGSEbgSJinKnV9OT3yrZ63oHMvbWjuLgg==&name=azfndn1%20function2";
                //const string myWebApiEndpoint = "https://azfndn1ipt.azurewebsites.net/api/HttpTrigger1?&name=azfndn1ipt%20foobar";                
                //const string myWebApiEndpoint = "https://localhost:44373/api/values"; // AzWebApp1
                //const string myWebApiEndpoint = "https://localhost:44351/api/todolist"; // TodoListService
                MyWebApiCallResults = await GetHttpContentWithToken(myWebApiEndpoint, authResult.AccessToken);
                DisplayBasicTokenInfo(authResult);
                CallMyWebApiVisibility = Visibility.Collapsed;
                MsalSignOutVisibility = Visibility.Visible;
            }

            isCallingMyWebApi = false;
        }

        private ICommand msalSignOutCommand;
        /// <summary>
        /// Setup the delegates for MsalSignOut
        /// </summary>
        public ICommand MsalSignOutCommand
        {
            get
            {
                return msalSignOutCommand ?? (msalSignOutCommand = 
                    new BasicCommand((arg) => MsalSignOut(), () => !IsMsalSigningOut()));
            }
        }

        private bool isMsalSigningOut;
        /// <summary>
        /// Whether or not the msal signing out process is in progress
        /// </summary>
        /// <returns></returns>
        public bool IsMsalSigningOut()
        {
            return isMsalSigningOut;
        }

        public async void MsalSignOut()
        {
            isMsalSigningOut = true;

            // execute msal signout here
            var accounts = await App.PublicClientApp.GetAccountsAsync(); 
            if (accounts.Any())
            {
                try
                {
                    await App.PublicClientApp.RemoveAsync(accounts.FirstOrDefault());
                    GraphApiCallResults = MyWebApiCallResults = string.Empty; TokenInfo = "User has been signed-out";
                    MsalAcquireTokenEnabled = true;
                    CallGraphApiVisibility = CallMyWebApiVisibility = Visibility.Visible;
                    MsalSignOutVisibility = Visibility.Collapsed;
                }
                catch (MsalException ex)
                {
                    TokenInfo = $"Error signing-out user: {ex.Message}";
                }
            }

            isMsalSigningOut = false;
        }

        /// <summary>
        /// Perform a microsoft authentication library [msal] acquire token silent/interactive sequence.
        /// </summary>
        /// <param name="scopes">The aad app registration scope permission you are requesting token for.</param>
        /// <param name="resultsNotifyPropertyChanged">The INotifyPropertyChanged enabled property to report results.</param>
        /// <returns>The authentication result from acquire token silent/interactive sequence.</returns>
        public async Task<AuthenticationResult> GetAuthResult(string[] scopes, string resultsNotifyPropertyChanged)
        {
            var app = App.PublicClientApp;
            var accounts = await app.GetAccountsAsync();
            AuthenticationResult authResult = null;

            try
            {
                authResult = await app.AcquireTokenSilentAsync(scopes, accounts.FirstOrDefault());
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
// public client application acquiretokenasync -> https://docs.microsoft.com/en-us/dotnet/api/microsoft.identity.client.publicclientapplication.acquiretokenasync?view=azure-dotnet
                    authResult = await App.PublicClientApp.AcquireTokenAsync(scopes);
//Sign in to your account | Microsoft | Sign in 
//Sorry, but we're having trouble signing you in. 
//AADSTS50194: Application '<application id>' is not configured as a multi-tenant application. Usage of the /common endpoint is not supported for such applications created after '10/15/2018'. Use a tenant-specific endpoint or configure the application to be multi-tenant. 

//App.xaml.cs | new PublicClientApplication(clientId) -> (clientId, authority);
//Constructor of the application. 
//authority: Authority of the security token service (STS) from which MSAL.NET will acquire the tokens. Usual authorities are: 
//https://login.microsoftonline.com/tenant/, where tenant is the tenant ID of the Azure AD tenant or a domain associated with this Azure AD tenant, in order to sign-in user of a specific organization only
//https://login.microsoftonline.com/common/ to sign-in users with any work and school accounts or Microsoft personal account 
//https://login.microsoftonline.com/organizations/ to sign-in users with any work and school accounts https://login.microsoftonline.com/consumers/ to sign-in users with only personal Microsoft account (live)
//Note that this setting needs to be consistent with what is declared in the application registration portal 
                }
                catch (MsalException msalex)
                {
                    resultsNotifyPropertyChanged = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                }
            }
            catch (Exception ex)
            {
                resultsNotifyPropertyChanged = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                //return; // now that part of helper function need to wait till end and return authResult
            }

            return authResult;
        }

        /// <summary>
        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="token">The token</param>
        /// <returns>String containing the results of the GET operation</returns>
        public async Task<string> GetHttpContentWithToken(string url, string token)
        {
            var httpClient = new System.Net.Http.HttpClient();
            //System.Net.Http.HttpResponseMessage response;
            try
            {
                var content = string.Empty;

                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // add the token in Authorization header
                //var response = await httpClient.GetAsync(url);

                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token); // add the token in Authorization header
                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode) content = await response.Content.ReadAsStringAsync();
                else content = $"a non-success status code of {response.StatusCode} was returned";
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Display basic information contained in the token
        /// </summary>
        private void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            TokenInfo = "";
            if (authResult != null)
            {
                TokenInfo += $"Username: {authResult.Account.Username}" + Environment.NewLine;
                TokenInfo += $"Token Expires: {authResult.ExpiresOn.ToLocalTime()}" + Environment.NewLine;
                TokenInfo += $"Access Token: {authResult.AccessToken}" + Environment.NewLine;
            }
        }
    }
}
