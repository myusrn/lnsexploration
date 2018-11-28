using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.Tests.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {

        }

        /// <summary>
        /// A collection of datagrid bindable data
        /// </summary>
        //public ObservableCollection<SomeDataTypeOnlyViewModel> SomeDataGridBindableData { get; set; }

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

        public void CallGraphApi()
        {
            isCallingGraphApi = true;

            // TODO: execute call graph api code here

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
