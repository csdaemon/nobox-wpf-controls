using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nobox.WPF.Controls.CommandConsole
{
    public class ConsoleContent : INotifyPropertyChanged
    {
        string consolePrompt = "NBX:> ";
        string consoleInput = string.Empty;
        ObservableCollection<ConsoleEntry> consoleOutput = new ObservableCollection<ConsoleEntry>() { new ConsoleEntry("[Nobox Command Console]") };

        public event EventHandler ConsoleContentUpdated;
        public void OnConsoleContentUpdated(EventArgs e)
        {
            EventHandler handler = ConsoleContentUpdated;
            handler?.Invoke(null, e);
        }

        public string ConsolePrompt
        {
            get
            {
                return consolePrompt;
            }
            set
            {
                consolePrompt = value;
                consolePrompt = consolePrompt.Trim() + " ";
                OnPropertyChanged("ConsolePrompt");
            }
        }

        public string ConsoleInput
        {
            get
            {
                return consoleInput;
            }
            set
            {
                consoleInput = value;
                OnPropertyChanged("ConsoleInput");
            }
        }

        public ObservableCollection<ConsoleEntry> ConsoleOutput
        {
            get
            {
                return consoleOutput;
            }
            set
            {
                consoleOutput = value;
                OnPropertyChanged("ConsoleOutput");
            }
        }

        public void RunCommand()
        {
            ConsoleOutput.Add(new ConsoleEntry(ConsolePrompt + ConsoleInput));

            try
            {
                CommandInvoker.Invoke(ConsoleInput);
            }
            catch (CommandInvokerException e)
            {
                ConsoleOutput.Add(new ConsoleEntry(ConsoleEntryType.ERROR, e.Message));
            }

            ConsoleInput = String.Empty;

            OnConsoleContentUpdated(new EventArgs());
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
