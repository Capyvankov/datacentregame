using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace dataCentre
{
    public static class Printer
    {
        public static void SystMes(string mes, string color, string type)
        {
            AnsiConsole.MarkupLine($"[{color}][[{type}]] {mes}[/]");
        }
    }
}
