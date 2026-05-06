using EvolutionSandbox.NeuralNetwork;
using EvolutionSandbox.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EvolutionSandbox.GameObjects
{
    internal class EvolutionManager : GameObject
    {
        List<Agent> currGen = new List<Agent>();
        List<Agent> AliveAgents = new List<Agent>();
        List<Agent> HigherHalf = new List<Agent>();
        FoodManager FoodMan;

        int AliveAgentsCountLast = -1;

        int GenCount = 0;
        float MedianScoreLastGen = 0;
        float AverageScoreLastGen = 0;
        float HighestScoreLastGen = 0;

        List<float> Medians = new List<float>();
        List<float> AverageScores = new List<float>();
        List<float> HighestScores = new List<float>();

        string GraphsDir = $"./{Configuration.Config.EnvName}/Graphs/";
        string CheckpointsDir = $"./{Configuration.Config.EnvName}/Checkpoints/";

        ulong PRGStateCheckpoint;
        

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

                    HigherHalf.Clear();
                    HigherHalf = currGen.GetRange(mid, currGen.Count - mid);
                    PRGStateCheckpoint = Utils.Random.State;

                    currGen.Clear();
                    AliveAgents.Clear();
                    FoodMan.Clear();

                    for (int i = 0; i < Configuration.Config.NumAgents; i++)
                    {
                        Agent newAgent = HigherHalf[i % HigherHalf.Count].DeepCopy();
                        currGen.Add(newAgent);
                        AliveAgents.Add(newAgent);
                        Program.SpawnGameObject(newAgent, false, false);
                    }
                    Medians.Add(MedianScoreLastGen);
                    AverageScores.Add(AverageScoreLastGen);
                    HighestScores.Add(HighestScoreLastGen);

                    if(Configuration.Config.GraphRate != 0)
                    {                    
                        if (GenCount % Configuration.Config.GraphRate == 0)
                        {
                            SaveGraph();
                        }
                    }

                    GenCount++;
                    UpdateStats();
                }
            }
        }

        void StartNew() // Starts new evolution based on config
        {
            #region Events
            Commands.OnGraphCommand += SaveGraph;
            Commands.OnCreateCheckpoint += CreateCheckpoint;
            #endregion

            Grid.Init(new Vector2Int((int)Configuration.Config.GridSizeX, (int)Configuration.Config.GridSizeY)); // Initialize size of grid

            UpdateStats();

            for (int i = 0; i < Configuration.Config.NumAgents; i++)
            {
                Agent agent = new Agent(new Vector2Int(Utils.Random.Next((int)Configuration.Config.GridSizeX),
                    Utils.Random.Next((int)Configuration.Config.GridSizeY)),
                    Guid.NewGuid(), this);
                currGen.Add(agent);
                AliveAgents.Add(agent);
                Program.SpawnGameObject(agent, false, false);
            }

            FoodManager foodManager = new FoodManager(Guid.NewGuid());
            Program.SpawnGameObject(foodManager);
            FoodMan = foodManager;
        }

        void StartFormCheckpoint() // Starts evolution basaed on checkpoint
        {

        }

        void CreateCheckpoint() // Creates an checkpoint
        {
            /*Things to save
                * Random generetor State
                * HigherHalf of agents (NNs)
                * stats (medians, averages, highests)
             */

            Directory.CreateDirectory(CheckpointsDir);
            string checkpointName = $"{GenCount.ToString()}-GenCheckpoint.json";

            var checkpoint = new
            {
                PRGState = PRGStateCheckpoint,
                Layers = from Agent in HigherHalf select Agent.GetNNCopy().GetLayersCopy(),
                Connections = from Agent in HigherHalf select Agent.GetNNCopy().GetConnectionsCopy(),
                Medians = Medians,
                AverageScores = AverageScores,
                HigherHalf = HigherHalf,
            };

            string jsonString = JsonSerializer.Serialize(checkpoint, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, MaxDepth = 256, WriteIndented = true });
            File.WriteAllText($"{CheckpointsDir}{checkpointName}", jsonString);

        }

        void SaveGraph()
        {
            ScottPlot.Plot myPlot = new();

            double[] generationsX = Enumerable.Range(1, Medians.Count).Select(x => (double)x).ToArray();
            double[] mediansY = Medians.Select(x => (double)x).ToArray();
            double[] averagesY = AverageScores.Select(x => (double)x).ToArray();
            double[] highestY = HighestScores.Select(x => (double)x).ToArray();

            ScottPlot.Plottables.Scatter scatterMedians = myPlot.Add.Scatter(generationsX, mediansY);
            scatterMedians.Color = ScottPlot.Colors.Blue;
            scatterMedians.LegendText = "Median";

            ScottPlot.Plottables.Scatter scatterAverages = myPlot.Add.Scatter(generationsX, averagesY);
            scatterAverages.Color = ScottPlot.Colors.Orange;
            scatterAverages.LegendText = "Averages";

            ScottPlot.Plottables.Scatter scatterHighest = myPlot.Add.Scatter(generationsX, highestY);
            scatterHighest.Color = ScottPlot.Colors.Green;
            scatterHighest.LegendText = "Highest score";

            myPlot.ShowLegend();
            string graphName = $"{Configuration.Config.EnvName}-{GenCount}Gen-TrainGraph.png";
            Directory.CreateDirectory(GraphsDir);
            myPlot.SavePng($"{GraphsDir}{graphName}", 1920, 1080);
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
