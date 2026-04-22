using System;
using System.IO;
using System.Text.Json;
namespace EvolutionSandbox
{
    internal static class Configuration
    {
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
                        Console.WriteLine("Enviroment with this name alerady exist, please choose another name - press any key to repeat action . . .");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Please enter an name - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an positive inteager - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an positive inteager - press any key to repeat action . . .");
                    Console.ReadKey();
                }
                else
                {
                    break;
                }
            }

            // Seed, FPS and APS
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter seed for the {config.EnvName} (X: {config.GridSizeX},Y: ?): ");
                temp = Console.ReadLine();
                if (!int.TryParse(temp, out config.Seed))
                {
                    Console.WriteLine("Please enter an inteager - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an positive inteager - press any key to repeat action . . .");
                    Console.ReadKey();
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.Write($"Enter actions per second for the {config.EnvName} (X: {config.GridSizeX},Y: {config.GridSizeX}): ");
                temp = Console.ReadLine();
                if (!uint.TryParse(temp, out config.APS))
                {
                    Console.WriteLine("Please enter an positive inteager - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an positive inteager - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an positive inteager - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                if (!double.TryParse(temp, out config.FoodEnergy))
                {
                    Console.WriteLine("Please enter an double - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                if (!double.TryParse(temp, out config.AgentMaxEnergy))
                {
                    Console.WriteLine("Please enter an double - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter an float - press any key to repeat action . . .");
                    Console.ReadKey();
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
                    Console.WriteLine("Please enter enviroment name. Press any key to retry action . . .");
                    Console.ReadKey();
                    continue;
                }

                LoadEnvFromConfig(envNameN);
                break;
            }
        }

        public static void LoadEnvFromConfig(string envName)
        {
            string configPath = $"{envName}/{envName}.conf";
            if (File.Exists(configPath))
            {
                string jsonString = File.ReadAllText(configPath);

                var options = new JsonSerializerOptions { IncludeFields = true };
                EnviromentConfig? configN = JsonSerializer.Deserialize<EnviromentConfig>(jsonString, options);

                if (configN != null)
                {
                    EnviromentConfig config = configN;
                    Console.WriteLine($"Loaded config for {config.EnvName}");
                }
                else
                {
                    Console.WriteLine("Config files failed to load.");
                    Console.WriteLine("Exitting . . .");
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine("Enviroment doesn't exist.");
                Console.WriteLine("Exitting . . .");
                Environment.Exit(1);
            }
        }


    }

    public class EnviromentConfig
    {
        public string EnvName;
        public uint GridSizeX;
        public uint GridSizeY;
        public int Seed;

        public uint FpsCap;
        public uint APS;

        public uint NumAgentsToStartWith;

        public uint MaxFoodInEnv;
        public float FoodSpawnRate;
        public double FoodEnergy;

        public double AgentMaxEnergy;
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