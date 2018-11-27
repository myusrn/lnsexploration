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
    }
}
