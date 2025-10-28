using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataCentre
{
    public static class GameOver
    {
        public static void GameOverFunc(string purpose)
        {
            var panel = new Panel($"" +
                $"Purpose : {purpose}\n" +
                $"SLA : {AllServers.SLA}");
            panel.Header = new PanelHeader($"Game over");
            panel.Border = BoxBorder.Rounded;
            panel.Padding = new Padding(2, 2, 2, 2);
            AnsiConsole.Profile.Capabilities.Ansi = true;
            AnsiConsole.Write(panel);
        }
    }
}
