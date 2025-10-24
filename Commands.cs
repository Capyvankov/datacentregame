using dataCentre.commands;
using dataCentre.server;
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
        void PrintDelegate(string[] command) => Console.WriteLine(string.Join(" ", command.Skip(1)));
        void SumDelegate(string[] command) => Console.WriteLine(int.Parse(command[1]) + int.Parse(command[2]));
        void StatusDelegate(Server server) => server.printServerStatus();

        public void readCommand(string inpCommand)
        {
            Server server = new Server { serverNum = 1 };
            Cmd printCmd = new Cmd { cmdName = "PRINT", commandDo = (Action<string[]>)PrintDelegate };
            Cmd sumCmd = new Cmd { cmdName = "SUM", commandDo = (Action<string[]>)SumDelegate };
            Cmd statusCmd = new Cmd { cmdName = "STATUS", commandDo = (Action<Server>)StatusDelegate };
            string[] command = inpCommand.Split(" ");
            if (printCmd.cmdName == command[0]) ((Action<string[]>)printCmd.commandDo)(command);
            else if (sumCmd.cmdName == command[0]) ((Action<string[]>)sumCmd.commandDo)(command);
            else if (statusCmd.cmdName == command[0]) ((Action<Server>)statusCmd.commandDo)(server);
        } 
    }
}
