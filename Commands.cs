using dataCentre.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dataCentre
{
    public class Commands
    {
        public void readCommand(string inpCommand)
        {
            PrintCmd print = new();
            SumCmd sum = new();
            string[] command = inpCommand.Split(" ");
            if (command[0] == "PRINT")
            {
                print.command(command);
            }
            if (command[0] == "SUM")
            {
                sum.command(command);
            }
        } 
    }
}
