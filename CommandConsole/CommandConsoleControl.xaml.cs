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
}
