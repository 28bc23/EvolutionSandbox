using EvolutionSandbox.NeuralNetwork;
using EvolutionSandbox.Utils;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        double StartGenAccumulator = 0.0;

        List<float> Medians = new List<float>();
        List<float> AverageScores = new List<float>();
        List<float> HighestScores = new List<float>();

        string GraphsDir = $"./{Configuration.Config.EnvName}/Graphs/";
        string CheckpointsDir = $"./{Configuration.Config.EnvName}/Checkpoints/";

        ulong PRGStateCheckpoint;

        double CurrentGenTime;


        public EvolutionManager(Guid id) : base(new Vector2Int(0, 0), id, 'M', GameObjectType.Manager, ConsoleColor.DarkRed)
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
                    StartGenAccumulator = Program.Accumulator;

                    foreach (Agent a in AliveAgents.ToArray())
                    {
                        Program.DestroyGameObject(a);
                    }
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

                    if (Configuration.Config.CheckpointRate != 0)
                    {
                        if (GenCount % Configuration.Config.CheckpointRate == 0)
                        {
                            CreateCheckpoint();
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

            Checkpoint? checkpoint = LoadCheckpoint();

            Grid.Init(new Vector2Int((int)Configuration.Config.GridSizeX, (int)Configuration.Config.GridSizeY)); // Initialize size of grid

            CreateLakes();

            UpdateStats();
            if (checkpoint == null)
            {
                FoodManager foodManager = new FoodManager(Utils.Random.NextGuid());
                Program.SpawnGameObject(foodManager);
                FoodMan = foodManager;

                for (int i = 0; i < Configuration.Config.NumAgents; i++)
                {
                    Agent agent = new Agent(new Vector2Int(Utils.Random.Next((int)Configuration.Config.GridSizeX),
                      Utils.Random.Next((int)Configuration.Config.GridSizeY)),
                     Utils.Random.NextGuid(), this);
                    currGen.Add(agent);
                    AliveAgents.Add(agent);
                    Program.SpawnGameObject(agent, false, false);
                }
            }
            else
            {
                ID = checkpoint.EvolutionManagerID;
                Utils.Random.Init(checkpoint.PRGState, false);
                Program.SetAccumulator(checkpoint.Accumulator);

                FoodManager foodManager = new FoodManager(checkpoint.FoodManagerID);
                Program.SpawnGameObject(foodManager);
                FoodMan = foodManager;

                for (int i = 0; i < Configuration.Config.NumAgents; i++)
                {
                    Agent agent = new Agent(new Vector2Int(Utils.Random.Next((int)Configuration.Config.GridSizeX),
                      Utils.Random.Next((int)Configuration.Config.GridSizeY)),
                    Utils.Random.NextGuid(), this, false);

                    NN tempNN = new NN(0, 0, false);
                    tempNN.SetLayers(checkpoint.Layers[i % checkpoint.Layers.Count]);
                    tempNN.SetConnections(checkpoint.Connections[i % checkpoint.Layers.Count]);

                    agent.SetNN(tempNN, true);

                    currGen.Add(agent);
                    AliveAgents.Add(agent);
                    Program.SpawnGameObject(agent, false, false);
                }

                Medians = checkpoint.Medians;
                AverageScores = checkpoint.AverageScores;
                HighestScores = checkpoint.HighestScores;
                GenCount = Medians.Count;
                MedianScoreLastGen = Medians[GenCount - 1];
                AverageScoreLastGen = AverageScores[GenCount - 1];
                HighestScoreLastGen = HighestScores[GenCount - 1];
            }

            CurrentGenTime = Configuration.Config.GenerationTime;
        }

        void CreateLakes()
        {
            int lakes = 10;
            int widthMax = 10;
            int widthMin = 2;
            int heightMax = 10;
            int heightMin = 2;
            for (int i = 0; i < lakes; i++)
            {
                List<Vector2Int> positions = new List<Vector2Int>();

                Vector2Int centerPos = new Vector2Int(Utils.Random.Next(Grid.GridSize.X), Utils.Random.Next(Grid.GridSize.Y));
                positions.Add(centerPos);

                int width = Utils.Random.Next(widthMin, widthMax);
                int height = Utils.Random.Next(heightMin, heightMax);

                Vector2 halfSize = new Vector2(width / 2.0f, height / 2.0f);

                for (int j = (int)(centerPos.X - halfSize.X); j <= (centerPos.X + halfSize.X); j++)
                {
                    if (j < 0)
                        continue;
                    if (j >= Grid.GridSize.X)
                        break;

                    for (int k = (int)(centerPos.Y - halfSize.Y); k <= (centerPos.Y + halfSize.Y); k++)
                    {
                        if (k < 0)
                            continue;
                        if (k >= Grid.GridSize.Y)
                            break;

                        Vector2Int pos = new Vector2Int(j, k);
                        if (InsideOfLake(centerPos, pos, halfSize))
                        {
                            positions.Add(pos);
                        }
                    }
                }

                foreach (Vector2Int pos in positions)
                {
                    Water water = new Water(pos, Utils.Random.NextGuid());
                    Program.SpawnGameObject(water, false, true);
                }
            }
        }

        public bool InsideOfLake(Vector2Int centerPos, Vector2Int pos, Vector2 halfSize)
        {
            halfSize.X = (halfSize.X == 0) ? 1 : halfSize.X;
            halfSize.Y = (halfSize.Y == 0) ? 1 : halfSize.Y;

            double p = Math.Pow(pos.X - centerPos.X, 2) / (double)Math.Pow(halfSize.X, 2) + Math.Pow(pos.Y - centerPos.Y, 2) / (double)Math.Pow(halfSize.Y, 2);
            return (p <= 1) ? true : false;
        }

        Checkpoint? LoadCheckpoint()
        {
            if (!Directory.Exists(CheckpointsDir))
            {
                Directory.CreateDirectory(CheckpointsDir);
            }

            DirectoryInfo di = new DirectoryInfo(CheckpointsDir);
            FileInfo[] checkpointsFi = di.GetFiles("*.json");
            if (checkpointsFi.Length > 0)
            {
                Console.WriteLine("Checkpoints found:");
                int latest = 0;
                foreach (FileInfo cpFi in checkpointsFi)
                {
                    int genNum;
                    if (int.TryParse(cpFi.Name.Split("-")[0], out genNum))
                    {
                        Console.Write($"{genNum} ");
                        if (genNum > latest)
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
                        JsonSerializerOptions options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, MaxDepth = 256, WriteIndented = true, IncludeFields = true };
                        Checkpoint? checkpoint = JsonSerializer.Deserialize<Checkpoint>(jsonString, options);
                        if (checkpoint != null)
                        {
                            Console.WriteLine("checkpoint loaded");
                            return checkpoint;
                        }
                    }
                    else
                        return null;
                }
            }
            else
                return null;
        }

        void CreateCheckpoint() // Creates a checkpoint
        {
            /*Things to save
                * Random generator State
                * HigherHalf of agents (NNs)
                * stats (medians, averages, highests)
                * UUID of evolution manager
                * UUID of food manager
                * Accumulator from Program.cs
             */

            Directory.CreateDirectory(CheckpointsDir);
            string checkpointName = $"{GenCount.ToString()}-GenCheckpoint.json";

            List<List<NNNode[]>> layers = new List<List<NNNode[]>>();
            List<List<NNConnection>> connections = new List<List<NNConnection>>();
            foreach (Agent a in HigherHalf)
            {
                NN nn = a.GetNNCopy();
                layers.Add(nn.GetLayersCopy());
                connections.Add(nn.GetConnectionsCopy());
            }

            Checkpoint checkpoint = new Checkpoint(PRGStateCheckpoint, layers, connections, Medians, AverageScores, HighestScores, ID, FoodMan.ID, StartGenAccumulator);

            string jsonString = JsonSerializer.Serialize(checkpoint, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, MaxDepth = 256, WriteIndented = true, IncludeFields = true });
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
        public ulong PRGState { get; set; }
        public List<List<NNConnection>> Connections { get; set; }
        public List<List<NNNode[]>> Layers { get; set; }
        public List<float> Medians { get; set; }
        public List<float> AverageScores { get; set; }
        public List<float> HighestScores { get; set; }
        public Guid EvolutionManagerID { get; set; }
        public Guid FoodManagerID { get; set; }
        public double Accumulator { get; set; }

        public Checkpoint() { }

        public Checkpoint(ulong pRGState, List<List<NNNode[]>> layers, List<List<NNConnection>> connections, List<float> medians, List<float> averageScores, List<float> highestScores, Guid evolutionManagerID, Guid foodManagerID, double accumulator)
        {
            PRGState = pRGState;
            Layers = layers;
            Connections = connections;
            Medians = medians;
            AverageScores = averageScores;
            HighestScores = highestScores;
            EvolutionManagerID = evolutionManagerID;
            FoodManagerID = foodManagerID;
            Accumulator = accumulator;
        }
    }
}
