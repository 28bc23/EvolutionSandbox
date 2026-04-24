using System;
using System.IO;
using System.Text.Json;
namespace EvolutionSandbox
{
    internal static class Configuration
    {
        public static EnviromentConfig Config;

        static string UintWarnMsg = "Please enter an positive inteager";
        static string FloatWarnMsg = "Please enter an float";
        public static void GenerateConfigForEnv()
        {
            EnviromentConfig config = new EnviromentConfig();

            #region Get User Input
            string? temp;

            // get name
            while (true)
            {
                Console.Clear();
                Console.Write("Enter name of the Enviroment: ");
                temp = Console.ReadLine();
                if (temp != null && temp != "" && temp != " " && temp[0] != '.')
                {
                    if (!Directory.Exists($"./{temp}"))
                    {
                        config.EnvName = temp;
                        Directory.CreateDirectory($"./{config.EnvName}");
                        break;
                    }
                    else
                    {
                        WaitForPress("Enviroment with this name alerady exist, please choose another name");
                    }
                }
                else
                {
                    WaitForPress("Please enter an name");
                }
            }

            // get grid size
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter X size of grid for the {config.EnvName}: ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out config.GridSizeX))
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
                Console.Write($"Enter Y size of grid for the {config.EnvName} (X: {config.GridSizeX},Y: ?): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out config.GridSizeY))
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
                Console.Write($"Enter seed for the {config.EnvName} (X: {config.GridSizeX},Y: ?): ");
                temp = Console.ReadLine();
                if (!ulong.TryParse(temp, out config.Seed))
                {
                    WaitForPress("Please enter an inteager");
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter fps cap for the {config.EnvName} (X: {config.GridSizeX},Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out config.FpsCap))
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
                Console.Write($"Enter ticks per second for the {config.EnvName} (X: {config.GridSizeX},Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out config.TPS))
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
                Console.Write($"With how many agents do you want do start the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out config.NumAgentsToStartWith))
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
                Console.Write($"Enter Maximum of food that can be spawned in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out config.MaxFoodInEnv))
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
                Console.Write($"Enter spawn rate of Food (food/second) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.FoodSpawnRate))
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
                Console.Write($"Enter energy of Food in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.FoodEnergy))
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
                Console.Write($"Enter enegry decrease rate of Agent (-energy/second) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.AgentEnergyDecreaseRate))
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
                Console.Write($"Enter max energy of Agent in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.AgentMaxEnergy))
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
                Console.Write($"Enter chance for weight mutation (0-1) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.WeightMutationChance))
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
                Console.Write($"Enter chance for bias mutation (0-1) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.BiasMutationChance))
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
                Console.Write($"Enter chance for connection split mutation (0-1) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.SplitMutationChance))
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
                Console.Write($"Enter chance for new node mutation (0-1) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.NewNodeMutationChance))
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
                Console.Write($"Enter chance for new connection mutation (0-1) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.NewConnectionMutationChance))
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
                Console.Write($"Enter min size for weight mutation (ex.: -1) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.WeightMutationSizeMin))
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
                Console.Write($"Enter max size for weight mutation (ex.: 1) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.WeightMutationSizeMax))
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
                Console.Write($"Enter min size for bias mutation (ex.: -1) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.BiasMutationSizeMin))
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
                Console.Write($"Enter max size for bias mutation (ex.: 1) in the {config.EnvName} (X: {config.GridSizeX}, Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!float.TryParse(temp, out config.BiasMutationSizeMax))
                {
                    WaitForPress(FloatWarnMsg);
                }
                else
                {
                    break;
                }
            }
            #endregion

            var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
            string fileName = $"./{config.EnvName}/{config.EnvName}.conf";
            string jsonString = JsonSerializer.Serialize(config, options);
            File.WriteAllText(fileName, jsonString);

            Console.WriteLine($"Config saved to {fileName}");

            LoadEnvFromConfig(config.EnvName);
        }

        public static void GetConfigFromUser()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Enter enviroment name of env you wanna load: ");
                string? envNameN = Console.ReadLine();
                Console.Clear();

                if (envNameN == "" || envNameN == null)
                {
                    WaitForPress("Please enter enviroment name");
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
                Console.WriteLine("Enviroment doesn't exist.");
                return false;
            }

            try
            {
                string jsonString = File.ReadAllText(configPath);
                var options = new JsonSerializerOptions { IncludeFields = true };
                EnviromentConfig? configN = JsonSerializer.Deserialize<EnviromentConfig>(jsonString, options);

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

        static void WaitForPress(string msg)
        {
            Console.WriteLine(msg);
            Console.WriteLine("Press any key to retry action . . .");
            Console.ReadKey();
        }
    }

    public class EnviromentConfig
    {
        public string EnvName;
        public uint GridSizeX;
        public uint GridSizeY;
        public ulong Seed;

        public uint FpsCap;
        public uint TPS;

        public uint NumAgentsToStartWith;

        public uint MaxFoodInEnv;
        public float FoodSpawnRate;
        public float FoodEnergy;

        public float AgentMaxEnergy;
        public float AgentEnergyDecreaseRate;

        public float WeightMutationChance;
        public float BiasMutationChance;
        public float SplitMutationChance;
        public float NewConnectionMutationChance;
        public float NewNodeMutationChance;

        public float WeightMutationSizeMin;
        public float WeightMutationSizeMax;
        public float BiasMutationSizeMin;
        public float BiasMutationSizeMax;
    }
}