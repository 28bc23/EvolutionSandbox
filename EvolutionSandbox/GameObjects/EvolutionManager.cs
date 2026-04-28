namespace EvolutionSandbox
{
    internal class EvolutionManager : GameObject
    {
        List<Agent> currGen = new List<Agent>();
        List<Agent> AliveAgents = new List<Agent>();
        FoodManager FoodMan;

        int AliveAgentsCountLast = -1;

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
            if (AliveAgentsCountLast != AliveAgents.Count)
            {
                UpdateStats();
            }


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

                    currGen.Clear();
                    AliveAgents.Clear();

                    for (int i = 0; i < Configuration.Config.NumAgents; i++)
                    {
                        Agent newAgent = higherHalf[i % higherHalf.Count].DeepCopy();
                        currGen.Add(newAgent);
                        AliveAgents.Add(newAgent);
                        Program.SpawnGameObject(newAgent, false, false);
                    }
                    GenCount++;
                    UpdateStats();
                }
            }
        }

        void StartNew() // Starts new evolution based on config
        {
            Grid.Init(new Vector2Int((int)Configuration.Config.GridSizeX, (int)Configuration.Config.GridSizeY)); // Initialize size of grid

            UpdateStats();

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
            FoodMan = foodManager;
        }

        void StartFormCheckpoint() // Starts evolution basaed on checkpoint
        {

        }

        void SaveProgress() // Creates an checkpoint
        {

        }

        public Vector2Int GetPosOfClosestFood(Vector2Int pos)
        {
            return FoodMan.GetPosOfClosestFood(pos);
        }

        public bool RemoveFromAliveList(Agent agent)
        {
            return AliveAgents.Remove(agent);
        }

        void UpdateStats()
        {
            Grid.SetUnderGridText($"""
                Generation: {GenCount}          
                Alive agents: {AliveAgents.Count}          
                Median of last gen scores: {MedianScoreLastGen}          
                Average of last gen scores: {AverageScoreLastGen}          
                Highest score of last gen: {HighestScoreLastGen}          
                """);
        }
    }
}