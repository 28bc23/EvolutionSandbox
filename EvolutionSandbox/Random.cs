namespace EvolutionSandbox
{
    internal static class Random
    {
        static System.Random rand = new System.Random(); // TODO: make own random generator so we can save its state

        public static void SetSeed(int seed)
        {
            rand = new System.Random(seed);
        }

        public static int RandomRangeInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        public static double RandomRangeDouble(double min, double max)
        {
            if (min > max)
                throw new ArgumentException("min must be less than or equal to max");

            return rand.NextDouble() * (max - min) + min;
        }

        public static bool Chance(float chance)
        {
            return rand.NextDouble() < chance;
        }
    }
}
