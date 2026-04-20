namespace EvolutionSandbox
{
    internal static class Configuration
    {
        public static void GenerateConfigForEnv()
        {
            string? envNameN; // string
            string? gridSizeXN; // uint
            string? gridSizeYN; // uint

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
        }

        public static void LoadEnvFromConfig()
        {
            
        }
    }
}