using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSandbox
{
    internal class Program
    {
        //Start
        static void Main(string[] args)
        {
            Grid.Init(10, 20);
            
            
            Loop();
        }

        //Loop
        static void Loop()
        {
            while (true)
            {
                Grid.DrawGrid();
            }
        }
    }
}
