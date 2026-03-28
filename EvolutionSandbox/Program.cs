using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace EvolutionSandbox
{
    internal class Program
    {
        static int FPScap = 10;

        static List<GameObject> GameObjects = new List<GameObject>();

        static Dictionary<Guid, Queue<Action>> Actions = new Dictionary<Guid, Queue<Action>>();

        //Game Start
        static void Main(string[] args)
        {
            Grid.Init(new Vector2Int(20, 10)); // Inicialize size of grid

            Agent agent = new Agent(new Vector2Int(10, 5), Guid.NewGuid());
            Agent agent2 = new Agent(new Vector2Int(5, 6), Guid.NewGuid());

            GameObjects.Add(agent);
            GameObjects.Add(agent2);
            
            GameLoop(); // Start Gmae loop
        }

        static void GameLoop()
        {
            DateTime time = DateTime.Now;
            int targetFrameTime = 1000 / FPScap; // How often should be showed new frame in ms
            while (true)
            {
                // Update and get actions form gameobjects
                foreach (GameObject gObj in GameObjects)
                {
                    gObj.Update();
                    Actions.Add(gObj.GID, gObj.GActions);
                }


                Dictionary<Guid, Queue<MoveAction>> moveActions = new Dictionary<Guid, Queue<MoveAction>>();

                foreach(KeyValuePair<Guid, Queue<Action>> gmActionsKVP in Actions)
                {
                    while (gmActionsKVP.Value.Count > 0)
                    {
                        Action gmAction = gmActionsKVP.Value.Dequeue();

                        switch (gmAction)
                        {
                            case MoveAction moveAction:
                                if(!moveActions.ContainsKey(gmActionsKVP.Key))
                                    moveActions.Add(gmActionsKVP.Key, new Queue<MoveAction>());
                                moveActions[gmActionsKVP.Key].Enqueue(moveAction);
                                break;
                            default:
                                break;
                        }
                    }
                }

                if(time == DateTime.Now.AddMilliseconds(targetFrameTime))
                {
                    Grid.DrawGrid();
                    time = DateTime.Now;
                }
                


                Actions.Clear();
            }
        }

        /* Getters and setters */

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
