using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EvolutionSandbox
{
    internal class Program
    {
        static int FPScap = 20;
        static double DeltaTime;

        //Game Start
        static void Main(string[] args)
        {
            Grid.Init(10, 20);
            
            
            Loop();
        }

        //Game Loop
        static void Loop()
        {
            Stopwatch stopwatch = new Stopwatch();
            int targetFrameTime = 1000 / FPScap;
            while (true)
            {
                stopwatch.Restart();
                Grid.DrawGrid();
                stopwatch.Stop();

                int elapsedMs = (int)stopwatch.ElapsedMilliseconds;
                int sleepTime = targetFrameTime - elapsedMs;

                if (sleepTime > 0)
                {
                    Thread.Sleep(sleepTime);
                }

                double totalFrameTimeMs = elapsedMs + (sleepTime > 0 ? sleepTime : 0);
                DeltaTime = totalFrameTimeMs / 1000.0;
            }
        }
    }
}
