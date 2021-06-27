using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nobox.WPF.Framework
{
    public class RelayCommand : ICommand
    {
        public string Name { get; private set; }
        public object Target { get; protected set; }
        public bool IsInitialized { get; private set; }

        private Action<object> methodToExecute;
        private Func<object,bool> canExecuteEvaluator;

        public static bool Enabled(object parameter) { return true; }
        public static bool Disabled(object parameter) { return false; }

        public RelayCommand()
        {
            IsInitialized = false;
        }

        public RelayCommand(string cmdName, Action<object> toExecute)
            : this(cmdName, toExecute, Enabled)
        {
        }

        public RelayCommand(string cmdName, Action<object> toExecute, Func<object,bool> canExecute)
        {
            InitializeCommand(cmdName, toExecute, canExecute);
        }

        public void InitializeCommand(string cmdName, Action<object> toExecute)
        {
            InitializeCommand(cmdName, toExecute, Enabled);
        }

        public void InitializeCommand(string cmdName, Action<object> toExecute, Func<object, bool> canExecute)
        {
            if (IsInitialized)
                throw new InvalidOperationException("RelayCommand Is Already Initialized.  Commands Can Only Be Initialized Once!");

            if (toExecute == null)
                throw new ArgumentNullException("toExecute");

            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            this.methodToExecute = toExecute;
            this.canExecuteEvaluator = canExecute;

            IsInitialized = true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (!IsInitialized)
                throw new InvalidOperationException(String.Format("RelayCommand {0} Has Not Been Initialized.", this.GetType()));

            return (canExecuteEvaluator != null && this.canExecuteEvaluator.Invoke(parameter));
        }

        public void Execute(object parameter)
        {
            if (!IsInitialized)
                throw new InvalidOperationException(String.Format("RelayCommand {0} Has Not Been Initialized.", this.GetType()));

            this.methodToExecute?.Invoke(parameter);
        }
    }
}
