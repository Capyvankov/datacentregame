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
            foreach (Server server in servers)
            {
                if (server.status == ServerStatus.Online)
                {
                    int result = random.Next(0, 16);
                    int index = random.Next(1, 4);
                    if (index == 1)
                    {
                        server.addTemp(result); 
                    }
                    else if (index == 2)
                    {
                        server.addLoad(result);
                    }
                    else if (index == 3) 
                    { 
                        server.addMemory(result); 
                    }
                    server.powerDraw = server.memoryUse * 10 + server.load * 10;
                    server.testWork();
                }

            }
        }
    }
}
