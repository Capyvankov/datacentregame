using dataCentre.commands;
using dataCentre.server;
using Spectre.Console;
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

        //void PrintDelegate(string[] command)
        //{
        //    lock (_consoleLock)
        //    {
        //        Console.WriteLine(string.Join(" ", command.Skip(1)));
        //    }
        //}

        //void SumDelegate(string[] command)
        //{
        //    int result = int.Parse(command[1]) + int.Parse(command[2]);
        //    lock (_consoleLock)
        //    {
        //        Console.WriteLine(result);
        //    }
        //}

        void StatusDelegate(Server server)
        {
            lock (_consoleLock)
            {
                server.printServerStatus();
            }
        }

        void RebootDelegate(Server server)
        {
            lock (_consoleLock)
            {
                server.reboot();
            }
        }

        void ShutdownDelegate(Server server)
        {
            lock (_consoleLock)
            {
                server.shutdown();
            }
        }

        void StartDelegate(Server server)
        {
            lock (_consoleLock)
            {
                server.switchOn();
            }
        }

        void CoolingDelegate(Server server, int coolant)
        {
            lock (_consoleLock)
            {
                server.coolantUse(coolant);
            }
        }
        void ReportDelegate()
        {
            lock (_consoleLock)
            {
                var table = new Table();

                // 3 колонки
                table.AddColumn("Server");
                table.AddColumn(new TableColumn("Status").Centered());
                table.AddColumn(new TableColumn("Power").RightAligned());

                // строки по 3 ячейки
                table.AddRow("srv1", $"{AllServers.server1.status}", $"{AllServers.server1.powerDraw}");
                table.AddRow("srv2", $"{AllServers.server2.status}", $"{AllServers.server2.powerDraw}");
                table.AddRow("srv3", $"{AllServers.server3.status}", $"{AllServers.server3.powerDraw}");
                table.AddRow("srv4", $"{AllServers.server4.status}", $"{AllServers.server4.powerDraw}");
                table.AddRow("srv5", $"{AllServers.server5.status}", $"{AllServers.server5.powerDraw}");

                table.Expand(); // по ширине консоли

                AnsiConsole.Write(table);
            }

        }


        public bool readCommand(string inpCommand)
        {
            if (string.IsNullOrWhiteSpace(inpCommand))
            {
                return false;
            }

            Cmd statusCmd = new Cmd { cmdName = "status", commandDo = (Action<Server>)StatusDelegate };
            Cmd rebootCmd = new Cmd { cmdName = "reboot", commandDo = (Action<Server>)RebootDelegate };
            Cmd shutdownCmd = new Cmd { cmdName = "shutdown", commandDo = (Action<Server>)ShutdownDelegate };
            Cmd startCmd = new Cmd { cmdName = "start", commandDo = (Action<Server>)StartDelegate };
            Cmd coolingCmd = new Cmd { cmdName = "cooling", commandDo = (Action<Server, int>)CoolingDelegate };
            Cmd reportCmd = new Cmd { cmdName = "report", commandDo = (Action)ReportDelegate };


            string[] command = inpCommand.Split(" ");
            if (statusCmd.cmdName == command[0])
            {
                if (command[1] == "1") ((Action<Server>)statusCmd.commandDo)(AllServers.server1);
                else if (command[1] == "2") ((Action<Server>)statusCmd.commandDo)(AllServers.server2);
                else if (command[1] == "3") ((Action<Server>)statusCmd.commandDo)(AllServers.server3);
                else if(command[1] == "4") ((Action<Server>)statusCmd.commandDo)(AllServers.server4);
                else if(command[1] == "5") ((Action<Server>)statusCmd.commandDo)(AllServers.server5);
                else Printer.SystMes($"Invalid argument {string.Join(" ", command.Skip(1))}", "#FF0000", "ERROR");
            }
            else if (rebootCmd.cmdName == command[0])
            {
                if (command[1] == "1") ((Action<Server>)rebootCmd.commandDo)(AllServers.server1);
                else if(command[1] == "2") ((Action<Server>)rebootCmd.commandDo)(AllServers.server2);
                else if(command[1] == "3") ((Action<Server>)rebootCmd.commandDo)(AllServers.server3);
                else if(command[1] == "4") ((Action<Server>)rebootCmd.commandDo)(AllServers.server4);
                else if(command[1] == "5") ((Action<Server>)rebootCmd.commandDo)(AllServers.server5);
                else Printer.SystMes($"Invalid argument {string.Join(" ", command.Skip(1))}", "#FF0000", "ERROR");
            }
            else if (shutdownCmd.cmdName == command[0])
            {
                if (command[1] == "1") ((Action<Server>)shutdownCmd.commandDo)(AllServers.server1);
                else if(command[1] == "2") ((Action<Server>)shutdownCmd.commandDo)(AllServers.server2);
                else if(command[1] == "3") ((Action<Server>)shutdownCmd.commandDo)(AllServers.server3);
                else if(command[1] == "4") ((Action<Server>)shutdownCmd.commandDo)(AllServers.server4);
                else if(command[1] == "5") ((Action<Server>)shutdownCmd.commandDo)(AllServers.server5);
                else Printer.SystMes($"Invalid argument {string.Join(" ", command.Skip(1))}", "#FF0000", "ERROR");
            }
            else if (startCmd.cmdName == command[0])
            {
                if (command[1] == "1") ((Action<Server>)startCmd.commandDo)(AllServers.server1);
                else if(command[1] == "2") ((Action<Server>)startCmd.commandDo)(AllServers.server2);
                else if(command[1] == "3") ((Action<Server>)startCmd.commandDo)(AllServers.server3);
                else if(command[1] == "4") ((Action<Server>)startCmd.commandDo)(AllServers.server4);
                else if(command[1] == "5") ((Action<Server>)startCmd.commandDo)(AllServers.server5);

                else Printer.SystMes($"Invalid argument {string.Join(" ", command.Skip(1))}", "#FF0000", "ERROR");
            }
            else if (coolingCmd.cmdName == command[0])
            {
                try
                {
                    int cooling = int.Parse(command[2]);
                    if(command[1] == "1") ((Action<Server, int>)coolingCmd.commandDo)(AllServers.server1, cooling);
                    else if(command[1] == "2") ((Action<Server, int>)coolingCmd.commandDo)(AllServers.server2, cooling);
                    else if(command[1] == "3") ((Action<Server, int>)coolingCmd.commandDo)(AllServers.server3, cooling);
                    else if(command[1] == "4") ((Action<Server, int>)coolingCmd.commandDo)(AllServers.server4, cooling);
                    else if(command[1] == "5") ((Action<Server, int>)coolingCmd.commandDo)(AllServers.server5, cooling);

                    else Printer.SystMes($"Invalid argument {string.Join(" ", command.Skip(1))}", "#FF0000", "ERROR");
                }
                catch (Exception)
                {
                    Printer.SystMes($"Invalid argument {string.Join(" ", command.Skip(1))}", "#FF0000", "ERROR");
                }
            }
            else if (reportCmd.cmdName == command[0])
            {
                ((Action)reportCmd.commandDo)();
            }
            else
            {
                Printer.SystMes($"No command name {command[0]}", "#FF0000", "ERROR");
            }

            return false;
        }
    }
}