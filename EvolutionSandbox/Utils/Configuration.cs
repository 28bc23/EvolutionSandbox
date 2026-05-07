using System.Text.Json;
namespace EvolutionSandbox.Utils
{
    internal static class Configuration
    {
        public static EnvironmentConfig Config;

        static string UintWarnMsg = "Please enter a positive integer";
        static string FloatWarnMsg = "Please enter a float";
        public static void GenerateConfigForEnv()
        {
            Config = new EnvironmentConfig();

            #region Get User Input
            string? temp;

            // get name
            while (true)
            {
                Console.Clear();
                Console.Write("Enter name of the Environment: ");
                temp = Console.ReadLine();
                if (temp != null && temp != "" && temp != " " && temp[0] != '.')
                {
                    if (!Directory.Exists($"./{temp}"))
                    {
                        Config.EnvName = temp;
                        Directory.CreateDirectory($"./{Config.EnvName}");
                        break;
                    }
                    else
                    {
                        WaitForPress("Environment with this name already exists, please choose another name");
                    }
                }
                else
                {
                    WaitForPress("Please enter a name");
                }
            }

            // get grid size
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter X size of grid for the {Config.EnvName}: ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out Config.GridSizeX))
                {
                    WaitForPress(UintWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter Y size of grid for the {Config.EnvName} (X: {Config.GridSizeX},Y: ?): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out Config.GridSizeY))
                {
                    WaitForPress(UintWarnMsg);
                }
                else
                {
                    break;
                }
            }

            // Seed, FPS and TPS
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter seed for the {Config.EnvName} (X: {Config.GridSizeX},Y: ?): ");
                temp = Console.ReadLine();
                if (!ulong.TryParse(temp, out Config.Seed))
                {
                    WaitForPress("Please enter an integer");
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter fps cap for the {Config.EnvName} (X: {Config.GridSizeX},Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out Config.FpsCap))
                {
                    WaitForPress(UintWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter ticks per second for the {Config.EnvName} (X: {Config.GridSizeX},Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out Config.TPS))
                {
                    WaitForPress(UintWarnMsg);
                }
                else
                {
                    break;
                }
            }

            // num agents
            while (true)
            {
                Console.Clear();
                Console.Write($"With how many agents do you want to train in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out Config.NumAgents))
                {
                    WaitForPress(UintWarnMsg);
                }
                else
                {
                    break;
                }
            }

            // Food
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter Maximum of food that can be spawned in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out Config.MaxFoodInEnv))
                {
                    WaitForPress(UintWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter spawn rate of Food (food/second) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.FoodSpawnRate))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter energy of Food in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.FoodEnergy))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }

            // Agent
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter energy decrease rate of Agent (energy/second) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}) (ex. 1): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.AgentEnergyDecreaseRate))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter max energy of Agent in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.AgentMaxEnergy))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter energy penalty for colliding with wall for agent in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}) (ex. 10): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.AgentWallCollisionEnergyPenalty))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter energy cost for step movement action of agent in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}) (ex. 5): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.AgentStepActionEnergyCost))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter energy cost for jump movement action of agent in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}) (ex. 10): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.AgentJumpActionEnergyCost))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter energy cost for no-move movement action of agent in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}) (ex. 0): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.AgentNoActionEnergyCost))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.Clear();
                Console.Write($"Graph save interval in generations (0 = disable): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out Config.GraphRate))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }

            // NN chances
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter chance for weight mutation (0-1) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.WeightMutationChance))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter chance for bias mutation (0-1) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.BiasMutationChance))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter chance for connection split mutation (0-1) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.SplitMutationChance))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter chance for new node mutation (0-1) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.NewNodeMutationChance))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter chance for new connection mutation (0-1) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.NewConnectionMutationChance))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }

            // NN mutation size
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter min size for weight mutation (ex.: -1) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.WeightMutationSizeMin))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter max size for weight mutation (ex.: 1) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.WeightMutationSizeMax))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter min size for bias mutation (ex.: -1) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.BiasMutationSizeMin))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter max size for bias mutation (ex.: 1) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.BiasMutationSizeMax))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            #endregion

            SaveConfig();
        }

        public static void GetConfigFromUser()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Enter environment name of env you wanna load: ");
                string? envNameN = Console.ReadLine();
                Console.Clear();

                if (envNameN == "" || envNameN == null)
                {
                    WaitForPress("Please enter environment name");
                    continue;
                }

                if (LoadEnvFromConfig(envNameN))
                    break;
                WaitForPress("");
            }
        }

        static bool LoadEnvFromConfig(string envName)
        {
            string configPath = $"{envName}/{envName}.conf";
            if (!File.Exists(configPath))
            {
                Console.WriteLine("environment doesn't exists.");
                return false;
            }

            try
            {
                string jsonString = File.ReadAllText(configPath);
                var options = new JsonSerializerOptions { IncludeFields = true };
                EnvironmentConfig? configN = JsonSerializer.Deserialize<EnvironmentConfig>(jsonString, options);

                if (configN != null)
                {
                    Config = configN;
                    Console.WriteLine($"Loaded config for {Config.EnvName}");
                    return true;
                }

                Console.WriteLine("Config file was empty or invalid.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load config: {ex.Message}");
                return false;
            }
        }

        public static bool SaveConfig()
        {
            var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
            string fileName = $"./{Config.EnvName}/{Config.EnvName}.conf";
            string jsonString = JsonSerializer.Serialize(Config, options);

            try
            {
                File.WriteAllText(fileName, jsonString);
            }catch (Exception ex)
            {
                Console.WriteLine($"Failed to save config: {ex.Message}");
                return false;
            }
            

            Console.WriteLine($"Config saved to {fileName}");

            return LoadEnvFromConfig(Config.EnvName);
        }

        static void WaitForPress(string msg)
        {
            Console.WriteLine(msg);
            Console.WriteLine("Press any key to retry action . . .");
            Console.ReadKey();
        }
    }

    public class EnvironmentConfig
    {
        public string EnvName = "no-name";
        public uint GridSizeX = 30;
        public uint GridSizeY = 30;
        public ulong Seed = 24150;

        public uint FpsCap = 30;
        public uint TPS = 30;
        public double MaxTicksPerFrame = 3;

        public uint NumAgents = 50;

        public uint MaxFoodInEnv = 100;
        public float FoodSpawnRate = 25;
        public float FoodEnergy = 50;

        public float AgentMaxEnergy = 500;
        public float AgentEnergyDecreaseRate = 1;
        public float AgentWallCollisionEnergyPenalty = 5;
        public float AgentStepActionEnergyCost = 5;
        public float AgentJumpActionEnergyCost = 10;
        public float AgentNoActionEnergyCost = 0;

        public uint GraphRate = 100;

        public float WeightMutationChance = 0.1f;
        public float BiasMutationChance = 0.05f;
        public float SplitMutationChance = 0.01f;
        public float NewConnectionMutationChance = 0.02f;
        public float NewNodeMutationChance = 0.01f;

        public float WeightMutationSizeMin = -0.2f;
        public float WeightMutationSizeMax = 0.2f;
        public float BiasMutationSizeMin = -0.1f;
        public float BiasMutationSizeMax = 0.1;
    }
}