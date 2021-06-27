using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nobox.WPF.Controls.CommandConsole
{
    public class CommandHandler
    {
        public CommandHandler()
        {
            SupportedCommands = new Dictionary<String, ConsoleCommand>();

        }

        public Dictionary<String,ConsoleCommand> SupportedCommands { get; private set; }

        protected void AddCommand(String call, ConsoleCommand cmd)
        {
            SupportedCommands.Add(call, cmd);
        }
    }
}
