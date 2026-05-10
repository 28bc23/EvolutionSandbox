using EvolutionSandbox;
using EvolutionSandbox.GameObjects;
using EvolutionSandbox.Utils;

namespace EvolutionSandbox
{
    internal class Program
    {
        static List<GameObject> GameObjects = new List<GameObject>();

        static Dictionary<Guid, Queue<Action>> ActionsQueue = new Dictionary<Guid, Queue<Action>>();

        public static double FixedDeltaTime { get; private set; }
        public static double Accumulator { get; private set; } = 0.0;
        static int TargetFrameTime = 1000 / (int)Configuration.Config.FpsCap; // How often should be showed new frame in ms

        //Game Start
        static void Main(string[] args)
        {
            Console.Clear();
            Console.Write("Do you wanna create new environment? [y/N]: ");
            string? input = Console.ReadLine();
            if (input != null && input.ToLower() == "y")
            {
                Configuration.GenerateConfigForEnv();
            }
            else
            {
                Configuration.GetConfigFromUser();
            }

            Utils.Random.Init(Configuration.Config.Seed, true);

            FixedDeltaTime = 1.0 / Configuration.Config.TPS;

            EvolutionManager evolutionManager = new EvolutionManager(Utils.Random.NextGuid());
            SpawnGameObject(evolutionManager);

            GameLoop(); // Start Gmae loop
        }

        static void GameLoop()
        {
            DateTime lastTimeFPS = DateTime.Now; // Last time for FPS limiter

            DateTime lastGameLoopTime = DateTime.Now; // Last time of game loop
            while (true)
            {
                /* calculate delta time */
                DateTime now = DateTime.Now;
                double frameTime = (now - lastGameLoopTime).TotalSeconds; // Get deltaTime (time from last game loop) in seconds
                lastGameLoopTime = now;

                Utils.Commands.ReadCommand();

                frameTime = Math.Clamp(frameTime, 0, FixedDeltaTime * Configuration.Config.MaxTicksPerFrame); /* clamping frameTime to disacoiate it from real time when the game takes much longer than fixedDeltaTime, 
                                                                                so we avoid spiral of death (situation where most of the time game frame takes much longer than fixedDeltaTime,
                                                                                so acumulator grows larger and it will never be lower than fixed delta time, so the game "freezes"/will not render any frames).
                                                                                By clamping the frameTime we tell the program that it taked for ex. 0.1 sec. insted of 2 sec. of real time.
                                                                                This way we artificialy delay game time from realtime and make it run in "slow motion"(bc. in game will pass 0.1 in 2 sec. of real time)*/

                frameTime *= Configuration.Config.TimeScale;

                Accumulator += frameTime;

                while (Accumulator >= FixedDeltaTime)
                {
                    // Update and get actions from gameobjects
                    GameObject[] gameObjects = GameObjects.ToArray();
                    foreach (GameObject gObj in gameObjects)
                    {
                        gObj.Update();
                        ActionsQueue[gObj.ID] = gObj.GetCopyOfActions();
                        gObj.ClearActions();
                    }


                    // Run actions
                    Dictionary<Guid, Queue<MoveAction>> goMoveActions = new Dictionary<Guid, Queue<MoveAction>>();

                    foreach (KeyValuePair<Guid, Queue<Action>> goActionsKVP in ActionsQueue)
                    {
                        while (goActionsKVP.Value.Count > 0)
                        {
                            Action gmAction = goActionsKVP.Value.Dequeue();

                            if (!GameObjects.Contains(gmAction.Initiator))
                                continue;

                            switch (gmAction)
                            {
                                case MoveAction moveAction:
                                    if (!goMoveActions.ContainsKey(goActionsKVP.Key))
                                        goMoveActions.Add(goActionsKVP.Key, new Queue<MoveAction>());
                                    goMoveActions[goActionsKVP.Key].Enqueue(moveAction);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (goMoveActions.Count > 0)
                        Grid.MoveObjects(goMoveActions);

                    ActionsQueue.Clear();

                    Accumulator -= FixedDeltaTime;
                }

                if ((DateTime.Now - lastTimeFPS).TotalMilliseconds >= TargetFrameTime)
                {
                    Grid.DrawGrid();
                    lastTimeFPS = DateTime.Now;
                }
            }
        }

        public static bool SpawnGameObject(GameObject gameObject, bool doNotSpawnWhenColliding = true, bool ignoreCollisions = false)
        {
            if (gameObject.GameObjectType == GameObjectType.Manager)
            {
                GameObjects.Add(gameObject);
                GameObjects.Sort();
                return true;
            }

            if (Grid.SpawnGameObject(gameObject, doNotSpawnWhenColliding, ignoreCollisions))
            {
                GameObjects.Add(gameObject);
                GameObjects.Sort();
                return true;
            }

            return false;
        }

        public static bool DestroyGameObject(GameObject gameObject)
        {
            if (Grid.RemoveGameObject(gameObject))
            {
                gameObject.OnDestroy();
                return GameObjects.Remove(gameObject);
            }
            return false;
        }

        public static void RecalculateFixedDeltaTime()
        {
            FixedDeltaTime = 1.0 / Configuration.Config.TPS;
        }

        public static void RecalculateTargetFrameTime()
        {
            TargetFrameTime = 1000 / (int)Configuration.Config.FpsCap;
        }

        public static void SetAccumulator(double accumulator)
        {
            if(accumulator < 0) return;

            Accumulator = accumulator;
        }
    }

    internal struct Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int(int x, int y)
        {
            X = x; Y = y;
        }
    }
}
