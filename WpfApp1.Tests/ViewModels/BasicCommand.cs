﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.Tests.ViewModels
{
    /// <summary>
    /// Simple ICommand implementation to bind commands to our ViewModels
    /// see "ViewModel BasicCommand ICommand" hits 
    /// </summary>
    public class BasicCommand : ICommand
    {
        private readonly Action<object> action;
        private readonly Func<bool> canExecute;

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public BasicCommand(Action<object> actionIn, Func<bool> canExecuteIn)
        {
            action = actionIn;
            canExecute = canExecuteIn;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;

            return canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
