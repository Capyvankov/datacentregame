using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataCentre
{
    public class Game
    {
        public void GoGame()
        {
            Commands com = new Commands();
            while (true)
            {
                string command = Console.ReadLine();
                com.readCommand(command);
                
            }
        }
    }
}
