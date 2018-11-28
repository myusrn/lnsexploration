using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
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
            GraphApiCallResults = TokenInfo = "initial value";
            SignOutVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// A collection of datagrid bindable data
        /// </summary>
        //public ObservableCollection<SomeDataTypeOnlyViewModel> SomeDataGridBindableData { get; set; }

        public string /*IObservable<string>*/ GraphApiCallResults { get; set; }
        public string  /*IObservable<string>*/ TokenInfo { get; set; }
        public Visibility /*IObservable<Visibility>*/ SignOutVisibility { get; set; }

        private ICommand msalRequestTokenCommand;
        /// <summary>
        /// Setup the delegates for MsalRequestToken
        /// </summary>
        public ICommand MsalRequestTokenCommand
        {
            get
            {
                return msalRequestTokenCommand ?? (msalRequestTokenCommand = 
                    new BasicCommand((arg) => MsalRequestToken(), () => !IsMsalRequestingToken()));
            }
        }

        private bool isMsalRequestingToken;
        /// <summary>
        /// Whether or not the msal requesting token process is in progress
        /// </summary>
        /// <returns></returns>
        public bool IsMsalRequestingToken()
        {
            return isMsalRequestingToken;
        }

        public void MsalRequestToken()
        {
            isMsalRequestingToken = true;

            // TODO: execute msal request token code here
            GraphApiCallResults = TokenInfo = "initial value updated";
            SignOutVisibility = Visibility.Visible;
            this.RaisePropertyChanged("GraphApiCallResults");
            this.RaisePropertyChanged("TokenInfo");
            this.RaisePropertyChanged("SignOutVisibility");

            isMsalRequestingToken = false;
        }

        private ICommand callGraphApiCommand;
        /// <summary>
        /// Setup the delegates for MsalRequestToken
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
        /// Whether or not the msal requesting token process is in progress
        /// </summary>
        /// <returns></returns>
        public bool IsCallingGraphApi()
        {
            return isCallingGraphApi;
        }

        // set the graph api endpoint to graph 'me' endpoint
        const string graphApiEndpoint = "https://graph.microsoft.com/v1.0/me";

        // set the scope for API call to user.read
        readonly string[] _scopes = new string[] { "user.read" };

        public async void CallGraphApi()
        {
            isCallingGraphApi = true;

            // execute call graph api code here
            AuthenticationResult authResult = null;

            var app = App.PublicClientApp;
            GraphApiCallResults = string.Empty;
            TokenInfo = string.Empty;

            var accounts = await app.GetAccountsAsync();

            try
            {
                authResult = await app.AcquireTokenSilentAsync(_scopes, accounts.FirstOrDefault());
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await App.PublicClientApp.AcquireTokenAsync(_scopes);
                }
                catch (MsalException msalex)
                {
                    GraphApiCallResults = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                }
            }
            catch (Exception ex)
            {
                GraphApiCallResults = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                return;
            }

            if (authResult != null)
            {
// TODO: find and add these methods
//                GraphApiCallResults = await GetHttpContentWithToken(graphApiEndpoint, authResult.AccessToken);
//                DisplayBasicTokenInfo(authResult);
                SignOutVisibility = Visibility.Visible;
            }

            isCallingGraphApi = false;
        }

        private ICommand msalSignOutCommand;
        /// <summary>
        /// Setup the delegates for MsalRequestToken
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
        /// Whether or not the msal requesting token process is in progress
        /// </summary>
        /// <returns></returns>
        public bool IsMsalSigningOut()
        {
            return isMsalSigningOut;
        }

        public void MsalSignOut()
        {
            isCallingGraphApi = true;

            // TODO: execute msal signout code here

            isCallingGraphApi = false;
        }
    }
}
