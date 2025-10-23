using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataCentre.commands
{
    public class PrintCmd
    {
        public string cmdName = "PRINT";
        public void command(string[] command)
        {
            Console.WriteLine(string.Join(" ", command.Skip(1)));
        }
    }
}
