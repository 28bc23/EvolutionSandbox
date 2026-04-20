using System;
using System.IO;
namespace EvolutionSandbox
{
    internal static class Configuration
    {
        public static void GenerateConfigForEnv()
        {
            string envName;
            uint gridSizeX;
            uint gridSizeY;
            int seed;

            uint fpsCap;
            uint aps;

            uint numAgentsToStartWith;

            uint maxFoodInEnv;
            float foodSpawnRate;
            double foodEnergy;

            double agentMaxEnergy;
            float agentEnergyDecreaseRate;

            float weightMutationChance;
            float biasMutationChance;
            float splitMutationChance;
            float newConnectionMutationChance;
            float newNodeMutationChance;

            float weightMutationSizeMin;
            float weightMutationSizeMax;
            float biasMutationSizeMin;
            float biasMutationSizeMax;

            string? temp;

            // get name
            while (true)
            {
                Console.Clear();
                Console.Write("Enter name of the Enviroment: ");
                temp = Console.ReadLine();
                if(temp != null && temp != "" && temp != " " && temp[0] != '.')
                {
                    if (!Directory.Exists($"./{temp}"))
                    {
                        envName = temp;
                        Directory.CreateDirectory($"./{envName}");
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
                Console.Write($"Enter X size of grid for the {envName}: ");
                temp = Console.ReadLine();
                if(!uint.TryParse(temp, out gridSizeX))
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
                Console.Write($"Enter Y size of grid for the {envName}(X: {gridSizeX},Y: ?): ");
                temp = Console.ReadLine();
                if(!uint.TryParse(temp, out gridSizeY))
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
                Console.Write($"Enter seed for the {envName}(X: {gridSizeX},Y: ?): ");
                temp = Console.ReadLine();
                if(!int.TryParse(temp, out seed))
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
                Console.Write($"Enter fps cap for the {envName}(X: {gridSizeX},Y: {gridSizeX}): ");
                temp = Console.ReadLine();
                if(!uint.TryParse(temp, out fpsCap))
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
                Console.Write($"Enter actions per second for the {envName}(X: {gridSizeX},Y: {gridSizeX}): ");
                temp = Console.ReadLine();
                if(!uint.TryParse(temp, out aps))
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
                Console.Write($"With how many agents do you want do start the {envName}(X: {gridSizeX},Y: {gridSizeX}): ");
                temp = Console.ReadLine();
                if(!uint.TryParse(temp, out numAgentsToStartWith))
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
                Console.Write($"Maximum of food that can be spawned in the {envName}(X: {gridSizeX},Y: {gridSizeX}): ");
                temp = Console.ReadLine();
                if(!uint.TryParse(temp, out maxFoodInEnv))
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
                Console.Write($"Maximum of food that can be spawned in the {envName}(X: {gridSizeX},Y: {gridSizeX}): ");
                temp = Console.ReadLine();
                if(!uint.TryParse(temp, out maxFoodInEnv))
                {
                    Console.WriteLine("Please enter an positive inteager - press any key to repeat action . . .");
                    Console.ReadKey();
                }
                else
                {
                    break;
                }
            }
        }

        public static void LoadEnvFromConfig()
        {
            
        }
    }
}