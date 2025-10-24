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
        private readonly object _consoleLock;

        public Commands(object consoleLock)
        {
            // Получаем объект блокировки из игры, чтобы синхронизировать вывод сообщений.
            _consoleLock = consoleLock;
        }

        void PrintDelegate(string[] command)
        {
            lock (_consoleLock)
            {
                Console.WriteLine(string.Join(" ", command.Skip(1)));
            }
        }

        void SumDelegate(string[] command)
        {
            int result = int.Parse(command[1]) + int.Parse(command[2]);
            lock (_consoleLock)
            {
                Console.WriteLine(result);
            }
        }

        void StatusDelegate(Server server)
        {
            lock (_consoleLock)
            {
                server.printServerStatus();
            }
        }

        public bool readCommand(string inpCommand)
        {
            if (string.IsNullOrWhiteSpace(inpCommand))
            {
                return false;
            }

            Cmd printCmd = new Cmd { cmdName = "print", commandDo = (Action<string[]>)PrintDelegate };
            Cmd sumCmd = new Cmd { cmdName = "sum", commandDo = (Action<string[]>)SumDelegate };
            Cmd statusCmd = new Cmd { cmdName = "status", commandDo = (Action<Server>)StatusDelegate };
            string[] command = inpCommand.Split(" ");
            // Специальная команда завершения игры: её обрабатывает сам Game.GoGame.
            if (string.Equals(command[0], "exit", StringComparison.OrdinalIgnoreCase))
            {
                lock (_consoleLock)
                {
                    Console.WriteLine("Exiting game...");
                }

                return true;
            }
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

            return false;
        }
    }
}