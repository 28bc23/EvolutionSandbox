using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace EvolutionSandbox
{
    internal class Program
    {
        static int FPScap = 10;
        public static int APS = 20; // Actions per second

        static List<GameObject> GameObjects = new List<GameObject>();

        static Dictionary<Guid, Queue<Action>> Actions = new Dictionary<Guid, Queue<Action>>();

        //Game Start
        static void Main(string[] args)
        {
            Grid.Init(new Vector2Int(20, 10)); // Inicialize size of grid

            Agent agent = new Agent(new Vector2Int(10, 5), Guid.NewGuid());
            Food food = new Food(new Vector2Int(5, 5), Guid.NewGuid());

            GameObjects.Add(agent);
            GameObjects.Add(food);
            
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
                    Actions[gObj.GID] = gObj.GActions;
                    gObj.ClearActions();
                }


                Dictionary<Guid, Queue<MoveAction>> goMoveActions = new Dictionary<Guid, Queue<MoveAction>>();

                foreach(KeyValuePair<Guid, Queue<Action>> goActionsKVP in Actions)
                {
                    while (goActionsKVP.Value.Count > 0)
                    {
                        Action gmAction = goActionsKVP.Value.Dequeue();

                        switch (gmAction)
                        {
                            case MoveAction moveAction:
                                if(!goMoveActions.ContainsKey(goActionsKVP.Key))
                                    goMoveActions.Add(goActionsKVP.Key, new Queue<MoveAction>());
                                goMoveActions[goActionsKVP.Key].Enqueue(moveAction);
                                break;
                            default:
                                break;
                        }
                    }
                }
                if(goMoveActions.Count > 0)
                    Grid.MoveObjects(goMoveActions);

                if((DateTime.Now - time).TotalMilliseconds >= targetFrameTime)
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
