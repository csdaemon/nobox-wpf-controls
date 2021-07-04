using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nobox.WPF.Controls.CommandConsole
{
    public class CommandInvoker
    {
        private static CommandInvoker staticInvoker = new CommandInvoker();

        private Dictionary<String, ConsoleCommand> commandCollection;

        private CommandInvoker()
        {
            commandCollection = new Dictionary<String, ConsoleCommand>();
        }

        public static void RegisterCommand(String call, ConsoleCommand command)
        {
            staticInvoker.commandCollection.Add(call, command);
        }

        public static void RegisterCommandHandler(CommandHandler cmdHandler)
        {
            foreach(KeyValuePair<String,ConsoleCommand> cmd in cmdHandler.SupportedCommands)
            {
                CommandInvoker.RegisterCommand(cmd.Key, cmd.Value);
            }
        }

        public static void Invoke(String commandString, bool canFail=true)
        {
            ConsoleCommand cmd = null;

            //String [] cmdList = commandString.Split(new Char[]{' '});
            
            Regex quotedSplit = new Regex("(?:^| )(\"(?:[^\"])*\"|[^ ]*)", RegexOptions.Compiled);

            List<string> splitList = new List<string>();
            string curr = null;
            foreach (Match match in quotedSplit.Matches(commandString))
            {
                curr = match.Value;
                if (curr.Length == 0)
                    splitList.Add("");
                else
                    splitList.Add(curr.Trim(new char[] { ' ','"' }));
            }

            List<string> cmdList = splitList.ToList();

            if (cmdList.Count <= 0)
                return;

            if(staticInvoker.commandCollection.TryGetValue(cmdList[0], out cmd))
            {
                cmd.Execute(cmdList);
            }
            else if(canFail)
            {
                throw new CommandInvokerException("Command Does Not Exist In Command Collection: " + commandString);
            }
        }
    }

    class CommandInvokerException : Exception
    {
        public CommandInvokerException() : base() { }
        public CommandInvokerException(String message) : base(message) { }
        public CommandInvokerException(String message, Exception inner) : base(message, inner) { }
        protected CommandInvokerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
