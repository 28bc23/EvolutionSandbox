using System;
using System.IO;
namespace EvolutionSandbox
{
    internal static class Configuration
    {
        public static void GenerateConfigForEnv()
        {
            string? envNameN; // string
            string? gridSizeXN; // uint
            string? gridSizeYN; // uint
            string? seedN; // int

            string? fpsCapN; // uint
            string? apsN; // uint

            string? numAgentsToStartWithN; // uint

            string? foodSpawnRateN; // float
            string? maxFoodInEnvN; // uint
            string? foodEnergyN; // double

            string? agentMaxEnergyN; // double
            string? agentEnergyDecreaseRateN; // float

            string? weightMutationChanceN; // float
            string? biasMutationChanceN; // float
            string? splitMutationChanceN; // float
            string? newConnectionMutationChanceN; // float
            string? newNodeMutationChanceN; // float

            string? weightMutationSizeMinN; // float
            string? weightMutationSizeMaxN; // float
            string? biasMutationSizeMinN; // float
            string? biasMutationSizeMaxN; // float

            string? temp;
            while (true)
            {
                Console.Clear();
                Console.Write("Enter name of the Enviroment: ");
                envNameN = Console.ReadLine();
                if(envNameN != null)
                {
                    Directory.CreateDirectory($"./{envNameN}");
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter an name - press any key to repeat action . . .");
                    Console.ReadKey();
                }
            }

                        while (true)
            {
                Console.Clear();
                Console.Write("Enter name of the Enviroment: ");
                envNameN = Console.ReadLine();
                if(envNameN != null)
                {
                    Directory.CreateDirectory($"./{envNameN}");
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter an name - press any key to repeat action . . .");
                    Console.ReadKey();
                }
            }
        }

        public static void LoadEnvFromConfig()
        {
            
        }
    }
}