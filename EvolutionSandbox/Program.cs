namespace EvolutionSandbox
{
    internal class Program
    {
        static uint FpsCap = 10;

        static List<GameObject> GameObjects = new List<GameObject>();

        static Dictionary<Guid, Queue<Action>> ActionsQueue = new Dictionary<Guid, Queue<Action>>();

        public static double FixedDeltaTime { get; private set; }
        static double accumulator = 0.0;

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

            Random.Init(Configuration.Config.Seed, true);

            FpsCap = Configuration.Config.FpsCap;

            FixedDeltaTime = 1.0 / Configuration.Config.TPS;

            EvolutionManager evolutionManager = new EvolutionManager(Guid.NewGuid());
            SpawnGameObject(evolutionManager);

            GameLoop(); // Start Gmae loop
        }

        static void GameLoop()
        {
            DateTime lastTimeFPS = DateTime.Now; // Last time for FPS limiter
            int targetFrameTime = 1000 / (int)FpsCap; // How often should be showed new frame in ms

            DateTime lastGameLoopTime = DateTime.Now; // Last time of game loop
            while (true)
            {
                /* calculate delta time */
                DateTime now = DateTime.Now;
                double frameTime = (now - lastGameLoopTime).TotalSeconds; // Get deltaTime (time from last game loop) in seconds
                lastGameLoopTime = now;

                accumulator += frameTime;

                while (accumulator >= FixedDeltaTime)
                {
                    // Update and get actions from gameobjects
                    GameObject[] gameObjects = GameObjects.ToArray();
                    foreach (GameObject gObj in gameObjects)
                    {
                        gObj.Update();
                        ActionsQueue[gObj.ID] = gObj.GetCopyOfActions();
                        gObj.ClearActions();
                    }


                    Dictionary<Guid, Queue<MoveAction>> goMoveActions = new Dictionary<Guid, Queue<MoveAction>>();

                    foreach (KeyValuePair<Guid, Queue<Action>> goActionsKVP in ActionsQueue)
                    {
                        while (goActionsKVP.Value.Count > 0)
                        {
                            Action gmAction = goActionsKVP.Value.Dequeue();

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

                    accumulator -= FixedDeltaTime;
                }

                if ((DateTime.Now - lastTimeFPS).TotalMilliseconds >= targetFrameTime)
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
                return true;
            }

            if (Grid.SpawnGameObject(gameObject, doNotSpawnWhenColliding, ignoreCollisions))
            {
                GameObjects.Add(gameObject);
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
