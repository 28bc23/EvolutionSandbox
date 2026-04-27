namespace EvolutionSandbox
{
    internal class EvolutionManager : GameObject
    {
        List<Agent> currGen = new List<Agent>();
        List<Agent> AliveAgents = new List<Agent>();

        int GenCount = 0;
        float MedianScoreLastGen = 0;
        float AverageScoreLastGen = 0;
        float HighestScoreLastGen = 0;

        public EvolutionManager(Guid id) : base(new Vector2Int(0, 0), id, 'M', GameObjectType.Manager)
        {
            StartNew();
        }

        public override void Update()
        {
            if (AliveAgents.Count == 0) // Generation finished
            {
                /* Evaluation */

                if (currGen.Count == 0)
                {
                    MedianScoreLastGen = 0;
                    AverageScoreLastGen = 0;
                    HighestScoreLastGen = 0;
                }
                else
                {
                    currGen.Sort();

                    // Median
                    int mid = currGen.Count / 2;
                    if (currGen.Count % 2 == 0)
                    {
                        MedianScoreLastGen = (currGen[mid - 1].GetScore() + currGen[mid].GetScore()) / 2.0f;
                    }
                    else
                    {
                        MedianScoreLastGen = currGen[mid].GetScore();
                    }

                    // Average
                    AverageScoreLastGen = 0;
                    foreach (Agent a in currGen)
                    {
                        AverageScoreLastGen += a.GetScore();
                    }
                    AverageScoreLastGen = AverageScoreLastGen / currGen.Count;

                    // Highest
                    HighestScoreLastGen = currGen[currGen.Count - 1].GetScore();

                    List<Agent> higherHalf = currGen.GetRange(mid, currGen.Count - mid);
                    List<Agent> newGen = new List<Agent>((int)Configuration.Config.NumAgents);

                    for (int i = 0; i < Configuration.Config.NumAgents; i++)
                    {
                        higherHalf[i % higherHalf.Count].MutateNN();
                        newGen.Add(higherHalf[i % higherHalf.Count]); // make deep copy insted
                    }
                    currGen = newGen;
                    // Spawn agents and add them to alive list
                }

                Environment.Exit(0); // insted: Take Higher half, copy them so there is max num of agents, mutate them and spawn new gen
            }
        }

        void StartNew() // Starts new evolution based on config
        {
            Grid.Init(new Vector2Int((int)Configuration.Config.GridSizeX, (int)Configuration.Config.GridSizeY)); // Initialize size of grid

            for (int i = 0; i < Configuration.Config.NumAgents; i++)
            {
                Agent agent = new Agent(new Vector2Int(Random.Next((int)Configuration.Config.GridSizeX),
                    Random.Next((int)Configuration.Config.GridSizeY)),
                    Guid.NewGuid(), this);
                currGen.Add(agent);
                AliveAgents.Add(agent);
                Program.SpawnGameObject(agent);
            }

            FoodManager foodManager = new FoodManager(Guid.NewGuid());
            Program.SpawnGameObject(foodManager);
        }

        void StartFormCheckpoint() // Starts evolution basaed on checkpoint
        {

        }

        void SaveProgress() // Creates an checkpoint
        {

        }

        public bool RemoveFromAliveList(Agent agent)
        {
            return AliveAgents.Remove(agent);
        }
    }
}