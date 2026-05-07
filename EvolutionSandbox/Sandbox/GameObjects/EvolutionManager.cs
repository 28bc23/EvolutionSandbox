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

        double CurrentGenTime;


        public EvolutionManager(Guid id) : base(new Vector2Int(0, 0), id, 'M', GameObjectType.Manager)
        {
            StartNew();
        }

        public override void Update()
        {
            CurrentGenTime -= Program.FixedDeltaTime;

            UpdateStats();

            if (AliveAgents.Count == 0 || CurrentGenTime <= 0) // Generation finished
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

                    if (Configuration.Config.GraphRate != 0)
                    {
                        if (GenCount % Configuration.Config.GraphRate == 0)
                        {
                            SaveGraph();
                        }
                    }

                    GenCount++;
                    UpdateStats();
                    CurrentGenTime = Configuration.Config.GenerationTime;
                }
            }
        }

        void StartNew() // Starts new evolution based on config
        {
            #region Events
            Commands.OnGraphCommand += SaveGraph;
            Commands.OnCreateCheckpoint += CreateCheckpoint;
            #endregion

            DirectoryInfo di = new DirectoryInfo(CheckpointsDir);
            FileInfo[] checkpointsFi = di.GetFiles("*.json");
            if (checkpointsFi.Length > 0)
            {
                Console.WriteLine("Checkpoints founded:");
                int latest = 0;
                foreach (FileInfo cpFi in checkpointsFi)
                {
                    int genNum;
                    if(int.TryParse(cpFi.Name.Split("-")[0], out genNum))
                    {
                        Console.Write($"{genNum} ");
                        if(genNum > latest)
                            latest = genNum;
                    }
                }
                while (true)
                {
                    Console.Write($"\nSelect checkpoint generation (default: {latest}; none to skip): ");
                    string? input = Console.ReadLine();
                    string genStr = "";
                    if (input != null)
                        genStr = input;
                    if (genStr.ToLower() != "none")
                    {
                        int gen;
                        if (!int.TryParse(genStr, out gen))
                            gen = latest;

                        string checkpointPath = $"{CheckpointsDir}{gen}-GenCheckpoint.json";

                        if (!File.Exists(checkpointPath))
                        {
                            Console.WriteLine("Checkpoint of this generation doesn't exist.\nPress any key to repeat . . .");
                            Console.ReadKey(intercept: true);
                            continue;
                        }

                        string jsonString = File.ReadAllText(checkpointPath);
                        JsonSerializerOptions options = new JsonSerializerOptions { MaxDepth = 256, WriteIndented = true };
                        Checkpoint? checkpoint = JsonSerializer.Deserialize<Checkpoint>(jsonString, options);
                        if( checkpoint != null )
                        {
                            Console.Write("checkpoint loaded");
                            Environment.Exit(0);
                        }
                    }
                    else
                        break;
                }                   
            }

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
            CurrentGenTime = Configuration.Config.GenerationTime;
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

            Checkpoint checkpoint = new Checkpoint(PRGStateCheckpoint, from Agent in HigherHalf select Agent.GetNNCopy().GetLayersCopy(), 
                from Agent in HigherHalf select Agent.GetNNCopy().GetConnectionsCopy(), Medians, AverageScores, HighestScores, HigherHalf);

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
            Remaining time: {CurrentGenTime}
            Alive agents: {AliveAgents.Count}
            Median of last gen scores: {MedianScoreLastGen}
            Average of last gen scores: {AverageScoreLastGen}
            Highest score of last gen: {HighestScoreLastGen}
            """);
        }
    }

    internal class Checkpoint
    {
        public ulong PRGState {  get; set; }
        public IEnumerable<NNNode[][]> Layers {  get; set; }
        public IEnumerable<NNConnection[]> Connections {  get; set; }
        public List<float> Medians {  get; set; }
        public List<float> AverageScores {  get; set; }
        public List<float> HighestScores {  get; set; }
        public List<Agent> HigherHalf {  get; set; }

        public Checkpoint(ulong pRGState, IEnumerable<NNNode[][]> layers, IEnumerable<NNConnection[]> connections, List<float> medians, List<float> averageScores, List<float> highestScores, List<Agent> higherHalf)
        {
            PRGState = pRGState;
            Layers = layers;
            Connections = connections;
            Medians = medians;
            AverageScores = averageScores;
            HighestScores = highestScores;
            HigherHalf = higherHalf;
        }
    }
}
