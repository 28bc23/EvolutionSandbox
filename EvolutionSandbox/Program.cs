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
            Grid.Init(10, 20); // Inicialize size of grid
            
            
            Loop(); // Start Gmae loop
        }

        //Game Loop
        static void Loop()
        {
            Stopwatch stopwatch = new Stopwatch();
            int targetFrameTime = 1000 / FPScap; //How much one frame should take in ms
            while (true)
            {
                stopwatch.Restart();


                Grid.DrawGrid();


                stopwatch.Stop();

                //FPScap to sleepTime calculation and sleep
                int elapsedMs = (int)stopwatch.ElapsedMilliseconds;
                int sleepTime = targetFrameTime - elapsedMs;

                if (sleepTime > 0)
                {
                    Thread.Sleep(sleepTime);
                }

                //DeltaTime claculation
                double totalFrameTimeMs = elapsedMs + (sleepTime > 0 ? sleepTime : 0); // DeltaTime is equal to time elapsed from last frame
                DeltaTime = totalFrameTimeMs / 1000.0; // Conversion from ms to s
            }
        }
    }
}
