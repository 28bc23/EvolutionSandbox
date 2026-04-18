namespace EvolutionSandbox
{
    internal static class Random
    {
        static System.Random rand = new System.Random();

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
