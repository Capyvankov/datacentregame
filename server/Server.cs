using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataCentre.server
{
    internal class Server
    {
        public required int serverNum;
        public int temp = 0;
        public int load = 0;
        public int memoryUse = 0;
        public int powerDraw = 0;
        public ServerStatus status = ServerStatus.Offline;

        //метода работы с данными серверов

        public void testWork()
        {
            if (temp >= 100) status = ServerStatus.Failed;
            if (load >= 100) status = ServerStatus.Offline;
            if (memoryUse >= 100) status = ServerStatus.Failed;
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

        // методы вывода

        public void printServerStatus()
        {
            var panel = new Panel($"status : {this.status}\n" +
                $"temp : {this.temp}\n" +
                $"load : {this.load} \n" +
                $"memory use : {this.memoryUse} \n" +
                $"power draw : {this.powerDraw} \n");
            panel.Header = new PanelHeader($"Server {serverNum} status");
            panel.Border = BoxBorder.Rounded;
            panel.Padding = new Padding(2, 2, 2, 2);
            AnsiConsole.Write(panel);
        }
    }

    enum ServerStatus
    {
        Online,   // работает
        Offline,  // выключен
        Failed    // сгорел или завис
    }
}
