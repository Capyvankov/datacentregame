using dataCentre.server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataCentre
{
    public static class EventGen
    {
        public static void Event()
        {
            Server[] servers = { AllServers.server1, AllServers.server2, AllServers.server3, AllServers.server4, AllServers.server5 };
            var random = new Random();
            int powerDrawNow = 0;
            foreach (Server server in servers)
            {
                if (server.status == ServerStatus.Online)
                {
                    int result = random.Next(0, 16);
                    server.addMemory(result);
                    server.load = server.memoryUse/5*2;
                    server.temp = server.temp + (10 * server.load) / 5;
                    server.powerDraw = server.memoryUse * 10 + server.load * 20;
                    server.testWork();
                }
                if (server.status == ServerStatus.Offline) AllServers.SLA -= 1;
                if (server.status == ServerStatus.Failed) AllServers.SLA -= 5;

                if (server.coolant)
                {
                    powerDrawNow += 100;
                    server.temp -= 5;
                }

                server.powerDraw += powerDrawNow;
                server.temp -= 1;

                if (server.temp < 20) server.temp = 20;
            }
            if (powerDrawNow > AllServers.powerDraw)
            {
                Printer.SystMes($"Power limit exceed!", "red", "ERROR");
                Printer.SystMes($"Overload protection triggered.", "cyan", "FAILSAFE");
                Printer.SystMes($"Emergency shutdown sequence engaged.", "cyan", "SYSTEM");
                AllServers.server1.shutdown();
                AllServers.server2.shutdown();
                AllServers.server3.shutdown();
                AllServers.server4.shutdown();
                AllServers.server5.shutdown();
            }
        }
    }
}