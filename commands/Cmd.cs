using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataCentre.commands
{
    public class Cmd
    {
        public required string cmdName;
        public required Delegate commandDo;
    }
}
