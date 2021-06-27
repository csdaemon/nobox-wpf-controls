using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nobox.WPF.Controls.CommandConsole
{
    public partial class CommandConsoleControl : UserControl
    {
        ConsoleContent dc = new ConsoleContent();

        public CommandConsoleControl()
        {
            InitializeComponent();

            PromptCommandHandler promptHandler = new PromptCommandHandler(dc);

            CommandInvoker.RegisterCommandHandler(promptHandler);

            dc.ConsoleContentUpdated += CommandConsoleContent_Updated;

            DataContext = dc;
            Loaded += CommandConsoleControl_Loaded;
        }

        void CommandConsoleContent_Updated(object sender, EventArgs e)
        {
            Scroller.ScrollToBottom();
        }

        void CommandConsoleControl_Loaded(object sender, RoutedEventArgs e)
        {
            InputBlock.KeyDown += InputBlock_KeyDown;
            InputBlock.Focus();
        }

        void InputBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dc.ConsoleInput = InputBlock.Text;
                dc.RunCommand();
                InputBlock.Focus();
                Scroller.ScrollToBottom();
            }
        }

        private void CommandConsoleControl_GotFocus(object sender, RoutedEventArgs e)
        {
            InputBlock.Focus();
        }
    }

    public enum ConsoleEntryType { MSG, ERROR }

    public class ConsoleEntry
    {
        public ConsoleEntry(String msg)
        {
            EntryType = ConsoleEntryType.MSG;
            EntryString = msg;
        }

        public ConsoleEntry(ConsoleEntryType type, String msg)
        {
            EntryType = type;
            EntryString = msg;
        }

        public String EntryString { get; set; }

        private ConsoleEntryType type;
        public ConsoleEntryType EntryType
        {
            get { return type; }
            set { type = value; }
        }

        public override string ToString()
        {
            return EntryString;
        }
    }

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
