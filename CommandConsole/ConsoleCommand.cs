using Nobox.WPF.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nobox.WPF.Controls.CommandConsole
{
    public sealed class ConsoleCommand
    {
        private RelayCommand TargetCommand;

        public ConsoleCommand(RelayCommand command)
        {
            TargetCommand = command;
        }

        public void Execute(List<string> commandString)
        {
            TargetCommand.Execute(commandString);
        }
    }
}

