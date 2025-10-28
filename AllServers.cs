using dataCentre.server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataCentre
{
    public static class AllServers
    {
        public static Server server1 = new Server { serverNum = 1 };
        public static Server server2 = new Server { serverNum = 2 };
        public static Server server3 = new Server { serverNum = 3 };
        public static Server server4 = new Server { serverNum = 4 };
        public static Server server5 = new Server { serverNum = 5 };
        public static int SLA = 100;
        public static int powerDraw = 8000;
    }
}
