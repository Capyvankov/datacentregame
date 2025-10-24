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
            
            Cmd printCmd = new Cmd { cmdName = "print", commandDo = (Action<string[]>)PrintDelegate };
            Cmd sumCmd = new Cmd { cmdName = "sum", commandDo = (Action<string[]>)SumDelegate };
            Cmd statusCmd = new Cmd { cmdName = "status", commandDo = (Action<Server>)StatusDelegate };
            string[] command = inpCommand.Split(" ");
            if (printCmd.cmdName == command[0])
            {
                ((Action<string[]>)printCmd.commandDo)(command);
            }
            else if (sumCmd.cmdName == command[0])
            {
                ((Action<string[]>)sumCmd.commandDo)(command);
            }
            else if (statusCmd.cmdName == command[0])
            {
                if (command[1] == "1") ((Action<Server>)statusCmd.commandDo)(AllServers.server1);
                if (command[1] == "2") ((Action<Server>)statusCmd.commandDo)(AllServers.server2);
                if (command[1] == "3") ((Action<Server>)statusCmd.commandDo)(AllServers.server3);
                if (command[1] == "4") ((Action<Server>)statusCmd.commandDo)(AllServers.server4);
                if (command[1] == "5") ((Action<Server>)statusCmd.commandDo)(AllServers.server5);
            }
        } 
    }
}