namespace EvolutionSandbox
{
    internal class Program
    {
        static int FPScap = 10;
        public static int APS = 20; // Actions per second

        static List<GameObject> GameObjects = new List<GameObject>();

        static Dictionary<Guid, Queue<Action>> Actions = new Dictionary<Guid, Queue<Action>>();

        

        static double DeltaTime;

        //Game Start
        static void Main(string[] args)
        {
            Grid.Init(new Vector2Int(20, 10)); // Inicialize size of grid

            Agent agent = new Agent(new Vector2Int(10, 5), Guid.NewGuid());
            FoodManager foodManager = new FoodManager(Guid.NewGuid());
            
            GameLoop(); // Start Gmae loop
        }

        static void GameLoop()
        {
            DateTime lastTimeFPS = DateTime.Now; // Last time for FPS limiter
            DateTime lastGameLoopTime = DateTime.Now; // Last time of game loop
            int targetFrameTime = 1000 / FPScap; // How often should be showed new frame in ms
            while (true)
            {
                /* calculat delta time */
                DateTime now = DateTime.Now;
                DeltaTime = (now - lastGameLoopTime).TotalSeconds; // Get deltatime (time from last game loop) in seconds
                lastGameLoopTime = now;

                // Update and get actions form gameobjects
                GameObject[] gameObjects = GameObjects.ToArray();
                foreach (GameObject gObj in gameObjects)
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

                if((DateTime.Now - lastTimeFPS).TotalMilliseconds >= targetFrameTime)
                {
                    Grid.DrawGrid();
                    lastTimeFPS = DateTime.Now;
                }
                


                Actions.Clear();
            }
        }

        public static bool SpawnGameObject(GameObject gameObject, Vector2Int pos, bool doNotSpawnWhenColliding = true, bool ignoreCollisions = false)
        {
            GameObjects.Add(gameObject);

            if(gameObject.GGameObjectType == GameObjectType.Manager)
                return true;

            return Grid.SpawnGameObject(gameObject, pos, doNotSpawnWhenColliding, ignoreCollisions);
        }

        public static bool DestroyGameObject(GameObject gameObject)
        {
            if(Grid.RemoveGameObject(gameObject))
                return GameObjects.Remove(gameObject);
            return false;
        }

        /* Getters and setters */

        public static double GDeltaTime
        {
            get { return DeltaTime; }
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
