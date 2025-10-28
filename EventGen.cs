using dataCentre.server;
using Spectre.Console;
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
                    int result = random.Next(0, 1);
                    server.addMemory(result);
                    server.load = server.memoryUse / 5 * 2;
                    TempChange(server);
                    server.powerDraw = server.memoryUse * 10 + server.load * 20 + server.coolant * 20;
                    server.testWork();
                }
                if (server.status == ServerStatus.Offline) AllServers.SLA -= 1;
                if (server.status == ServerStatus.Failed) AllServers.SLA -= 5;
            }
            if (powerDrawNow > AllServers.powerDraw)
            {
                Printer.SystMes($"Power limit exceed!", "#FF0000", "ERROR");
                Printer.SystMes($"Overload protection triggered. Emergency shutdown sequence engaged.", "#00BFFF", "SYSTEM");
                AllServers.server1.shutdown();
                AllServers.server2.shutdown();
                AllServers.server3.shutdown();
                AllServers.server4.shutdown();
                AllServers.server5.shutdown();
            }

            int counterFailed = 0;
            foreach (Server server in servers) if (server.status == ServerStatus.Failed) counterFailed++;
            if (counterFailed >= 3) GameOver.GameOverFunc($"Data center destroyed. {counterFailed} servers permanently lost.");
        }

        public static void TempChange(Server server)
        {
            // константы — подгоняй балансом
            int ambient = 25; //температура комнаты

            int upDiv = 8;   // чем больше — тем медленнее нагрев
            int maxUp = 10;  // максимум роста за тик

            int downDiv = 3;   // чем больше — тем медленнее естественное охлаждение
            int baseCool = 1;  // естественное остывание без охлада
            int coolPerLevel = 2; // бонус за уровень охлаждения (0..5)
            int maxDown = 15;  // максимум падения за тик

            int Tt = 10 * server.load; // целвая темпераутра - мечта сервера

            if (server.temp < Tt)
            {
                int step = Math.Max(1, (Tt - server.temp) / upDiv - coolPerLevel * server.coolant);
                server.temp += Math.Min(maxUp, step);                 // медленный нагрев
            }
            else if (server.temp > Tt)
            {
                int step = Math.Max(1, (server.temp - Tt) / downDiv);
                step += baseCool + coolPerLevel * server.coolant; // быстрый отвод тепла
                server.temp -= Math.Min(maxDown, step);
            }

            if (server.status == ServerStatus.Offline)
                server.temp = Math.Max(ambient, server.temp - 2);             // оффлайн — остывает ещё быстрее

            server.temp = Math.Max(ambient, server.temp);                      // ниже комнаты не падаем
        }
    }
}