using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace dataCentre.server
{
    public class Server
    {
        public required int serverNum;
        public int temp = 0;    //температура
        public int load = 0;    //нагрузка на проц
        public int memoryUse = 0; //использвоания оперативки
        public int powerDraw = 0; //энергопотребление
        public int coolant = 0;
        public ServerStatus status = ServerStatus.Offline;

        //метода работы с данными серверов

        public void testWork()
        {
            if (temp >= 100)
            {
                Printer.SystMes($"srv{this.serverNum}: Thermal runaway detected.", "#8B0000", "CRITICAL");
                serverDie();

            }
            if (load >= 100) 
            {
                Printer.SystMes($"srv{this.serverNum}: CPU overload detected.", "#8B0000", "CRITICAL");
                serverDie();
            }
            if (memoryUse >= 100)
            {
                Printer.SystMes($"srv{this.serverNum}: Memory usage critical!", "red", "ERROR");
                shutdown();
            }
        }

        public void addTemp(int temp)
        {
            this.temp += temp;
        }

        public void addLoad(int load)
        {
            this.load += load;
        }

        public void addMemory(int memory)
        {
            this.memoryUse += memory;
        }

        public void addPower(int powerDraw)
        {
            this.powerDraw += powerDraw;
        }
        public void coolantUse(int level)
        {
            if (level > 5 ||  level < 0)
            {
                Printer.SystMes("Cooling parameter out of range. Valid levels: 0..5.", "red", "ERROR");
            }
            else
            {
                this.coolant = level;
            }          
        }

        public void shutdown()
        {
            if (status != ServerStatus.Failed)
            {
                this.status = ServerStatus.Offline;
                this.load = 0;
                this.memoryUse = 0;
                this.powerDraw = 0;
                this.coolant = 0;
                Printer.SystMes($"srv{this.serverNum} offline", "yellow", "WARN");
            }
        }

        public void switchOn()
        {
            if (status != ServerStatus.Failed)
            {
                this.status = ServerStatus.Online;
                this.load = 20;
                this.memoryUse = 20;
                this.powerDraw = this.memoryUse * 10 + this.load * 20;
                this.coolant = 0;
                Printer.SystMes($"srv{this.serverNum} online", "yellow", "WARN");
            }
        }

        public void reboot()
        {
            AnsiConsole.Progress()
           .Start(ctx =>
           {
               var task = ctx.AddTask("Reboot");
               task.MaxValue = 100;

               for (int i = 0; i <= 100; i++)
               {
                   task.Value = i;
                   Thread.Sleep(30); // 100 * 30 мс = ~3 сек
               }
           });
            Printer.SystMes("Rebooting is sucsesfull", "cyan", "INFO");
            this.memoryUse = this.memoryUse / 2;
        }

        public void serverDie()
        {
            if (status != ServerStatus.Failed)
            {
                this.status = ServerStatus.Failed;
                this.load = 0;
                this.memoryUse = 0;
                this.powerDraw = 0;
                this.coolant = 0;
                Printer.SystMes($"srv{this.serverNum} : hardware integrity lost", "#8B0000", "CRITICAL");
            }
        }

        // методы вывода

        public void printServerStatus()
        {
            var panel = new Panel($"status : {this.status}\n" +
                $"temp : {this.temp}\n" +
                $"load : {this.load} \n" +
                $"memory use : {this.memoryUse} \n" +
                $"power draw : {this.powerDraw} \n" +
                $"coolant : {this.coolant}");
            panel.Header = new PanelHeader($"srv{serverNum} status");
            panel.Border = BoxBorder.Rounded;
            panel.Padding = new Padding(2, 2, 2, 2);
            AnsiConsole.Profile.Capabilities.Ansi = true;
            AnsiConsole.Write(panel);
        }     
    }

    public enum ServerStatus
    {
        Online,   // работает
        Offline,  // выключен
        Failed    // сгорел или завис
    }
}
