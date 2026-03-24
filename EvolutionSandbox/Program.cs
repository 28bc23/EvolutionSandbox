using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace EvolutionSandbox
{
    internal class Program
    {
        static int FPScap = 100;
        static double DeltaTime;

        static List<GameObject> GameObjects = new List<GameObject>();

        //Game Start
        static void Main(string[] args)
        {
            Grid.Init(new Vector2Int(20, 10)); // Inicialize size of grid

            Agent agent = new Agent(new Vector2Int(10, 5));
            Agent agent2 = new Agent(new Vector2Int(5, 6));

            GameObjects.Add(agent);
            GameObjects.Add(agent2);
            
            GameLoop(); // Start Gmae loop
        }

        static void GameLoop()
        {
            Stopwatch stopwatch = new Stopwatch();
            int targetFrameTime = 1000 / FPScap; //How much one frame should take in ms
            while (true)
            {
                stopwatch.Restart();

                foreach (GameObject obj in GameObjects) // TODO: Insted of making action instantly take only information of the action gameObject wants to make and after we have all information execute all at one to make it fair fore every gameObject
                {
                    obj.Update();
                }

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

    internal class Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int(int x, int y)
        {
            X = x; Y = y;
        }
    }
}
