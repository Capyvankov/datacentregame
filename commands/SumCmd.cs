using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataCentre.commands
{
    public class SumCmd
    {
        public string cmdName = "SUM";

        public void command(string[] command)
        {
            Console.WriteLine(int.Parse(command[1]) + int.Parse(command[2]));
        }
    }
}
