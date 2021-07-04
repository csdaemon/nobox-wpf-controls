using Nobox.WPF.Controls;
using Nobox.WPF.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nobox.WPF.Controls.CommandConsole
{
    public class PromptCommandHandler : CommandHandler
    {
        protected ConsoleContent TargetConsole { get; set; }

        private class PromptCommandExecutor : RelayCommand
        {
            protected ConsoleContent TargetConsole { get; set; }
            public PromptCommandExecutor(ConsoleContent target) 
                : base()
            {
                TargetConsole = target;
                InitializeCommand("PromptCommandExecutor", PromptCommandExecute);
            }

            private void PromptCommandExecute(object param)
            {
                TargetConsole.ConsolePrompt = (param as List<string>)[1];
            }
        }

        private class EchoCommandExecutor : RelayCommand
        {
            protected ConsoleContent TargetConsole { get; set; }
            public EchoCommandExecutor(ConsoleContent target) 
                : base()
            {
                TargetConsole = target;
                InitializeCommand("EchoCommandExecutor", EchoCommandExecute);
            }

            private void EchoCommandExecute(object param)
            {
                List<string> commandString = param as List<string>;

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TargetConsole.ConsoleOutput.Add(new ConsoleEntry(string.Join(" ", commandString.Skip(1))));
                }));
            }
        }

        public PromptCommandHandler(ConsoleContent consoleControl)
        {
            TargetConsole = consoleControl;

            //AddCommand("PROMPT", new PromptCommandExecutor(TargetConsole));
            //AddCommand("ECHO", new EchoCommandExecutor(TargetConsole));

            AddCommand("PROMPT", new ConsoleCommand(new PromptCommandExecutor(TargetConsole)));
            AddCommand("ECHO", new ConsoleCommand(new EchoCommandExecutor(TargetConsole)));
            
        }
    }
}
