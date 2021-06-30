using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nobox.WPF.Controls.CommandConsole
{
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
}
