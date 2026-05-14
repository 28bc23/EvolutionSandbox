using System.Diagnostics;
using System.Text.Json;
using System.Runtime.InteropServices;
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
                    WaitForPress(UintWarnMsg += " (64 bit)");
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
                if (!int.TryParse(temp, out Config.FpsCap))
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
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter time scale for the {Config.EnvName} (X: {Config.GridSizeX},Y: {Config.GridSizeX}) (ex. 10.0): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.TimeScale))
                {
                    WaitForPress(FloatWarnMsg);
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
                if (!int.TryParse(temp, out Config.NumAgents))
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
                Console.Write($"How long should one generation take (seconds) in the {Config.EnvName} (X: {Config.GridSizeX}, Y: {Config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.GenerationTime))
                {
                    WaitForPress(FloatWarnMsg);
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
                if (!int.TryParse(temp, out Config.MaxFoodInEnv))
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
                if (!int.TryParse(temp, out Config.GraphRate))
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
                Console.Write($"Checkpoint save interval in generations (0 = disable): ");
                temp = Console.ReadLine();
                if (!int.TryParse(temp, out Config.CheckpointRate))
                {
                    WaitForPress(UintWarnMsg);
                }
                else
                {
                    break;
                }
            }

            // Score Coefs
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter coefficient for FoodScore (ex. 0.6): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.FoodScoreCoef))
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
                Console.Write($"Enter coefficient for energy bonus (ex. 0.2): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.EnergyBonusCoef))
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
                Console.Write($"Enter coefficient for close to food bonus (ex. 0.1): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.CloseToFoodBonusCoef))
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
                Console.Write($"Enter coefficient for center bonus (ex. 0.1): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out Config.CenterBonusCoef))
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

            Console.Clear();
            Console.Write("Would you like to adjust your configuration manually? [y/N]: ");
            if (Console.ReadLine().ToLower() == "y")
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    try
                    {
                        Process process = Process.Start("notepad.exe", $"./{Config.EnvName}/{Config.EnvName}.conf");
                        process?.WaitForExit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"The Notepad program was not found.\nIf you still want to edit your configuration, it is located at ./{Config.EnvName}/{Config.EnvName}.conf\nWhen you are finished, press any key to continue . . .");
                        Console.ReadKey();
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    try
                    {
                        Process process = Process.Start("vim", $"./{Config.EnvName}/{Config.EnvName}.conf");
                        process?.WaitForExit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"The vim program was not found.\nIf you still want to edit your configuration, it is located at ./{Config.EnvName}/{Config.EnvName}.conf\nWhen you are finished, press any key to continue . . .");
                        Console.ReadKey();
                    }
                }
                LoadEnvFromConfig(Config.EnvName);
            }
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
                Console.WriteLine("environment doesn't exist.");
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

                    if (Config.MaxTicksPerFrame < 1)
                    {
                        Config.MaxTicksPerFrame = 1;
                        SaveConfig();
                    }

                    if (Config.EnvName != envName)
                    {
                        Config.EnvName = envName;
                        SaveConfig();
                    }

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
            }
            catch (Exception ex)
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

        public int FpsCap = 30;
        public uint TPS = 1;
        public float TimeScale = 10;
        public double MaxTicksPerFrame = 3;

        public int NumAgents = 50;
        public float GenerationTime = 120.0f;

        public int MaxFoodInEnv = 100;
        public float FoodSpawnRate = 25;
        public float FoodEnergy = 50;

        public float AgentMaxEnergy = 500;
        public float AgentEnergyDecreaseRate = 1;
        public float AgentWallCollisionEnergyPenalty = 5;
        public float AgentStepActionEnergyCost = 5;
        public float AgentJumpActionEnergyCost = 10;
        public float AgentNoActionEnergyCost = 0;

        public char GrassCharacter = '•';
        public char AgentCharacter = '*';
        public char FoodCharacter = 'X';
        public char WaterCharacter = 'W';

        public ConsoleColor GrassColor = ConsoleColor.Green;
        public ConsoleColor AgentColor = ConsoleColor.White;
        public ConsoleColor FoodColor = ConsoleColor.Yellow;
        public ConsoleColor UnderGridTextColor = ConsoleColor.Gray;
        public ConsoleColor WaterColor = ConsoleColor.Blue;

        public int GraphRate = 100;
        public int CheckpointRate = 100;

        public float FoodScoreCoef = .6f;
        public float EnergyBonusCoef = .2f;
        public float CloseToFoodBonusCoef = .1f;
        public float CenterBonusCoef = .1f;

        public float WeightMutationChance = 0.1f;
        public float BiasMutationChance = 0.05f;
        public float SplitMutationChance = 0.01f;
        public float NewConnectionMutationChance = 0.02f;
        public float NewNodeMutationChance = 0.01f;

        public float WeightMutationSizeMin = -0.2f;
        public float WeightMutationSizeMax = 0.2f;
        public float BiasMutationSizeMin = -0.1f;
        public float BiasMutationSizeMax = 0.1f;
    }
}