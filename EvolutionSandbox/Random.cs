namespace EvolutionSandbox
{
    internal static class Random
    {
        #region PCG-XSH-RR random number generator - https://en.wikipedia.org/wiki/Permuted_congruential_generator
        public static ulong State { get; private set; }
        const ulong Multiplier = 6364136223846793005u;
        const ulong Increment = 1442695040888963407u;

        public static void Init(ulong initVal, bool bIsSeed)
        {
            if (bIsSeed)
            {
                State = initVal + Increment;
                Next(); // first move for better generation
            }
            else
            {
                State = initVal;
            }
        }

        static uint rot(uint x, int r) // rotation of bits to right
        {
            return x >> r | x << (-r & 31); // first part shifts x to right by r. second part shifts x to left by 32 - r so ve get bits that we lost in first part. By or op. ve combine these parts and get rotation to right.
        }

        static uint Next()
        {
            ulong x = State;
            uint count = (uint)(x >> 59); // shifts x to right by 59 so we get num between 0 - 31 for bit rotation later
            State = x * Multiplier + Increment; // LCG
            x ^= x >> 18; // Xorshift for better random num from LCG
            return rot((uint)(x >> 27), (int)count); // result of the PCG
        }
        #endregion

        public static int Next(int max) // gen random number between 0 - max (except max)
        {
            if (0 >= max)
                throw new ArgumentException("max must be positive number");
            return (int)(((ulong)Next() * (uint)max) >> 32); // >> 32 - we throw away lower half to make it int32
        }

        public static int Next(int min, int max) // gen random number between min - max (except max)
        {
            if (min > max)
                throw new ArgumentException("min must be less than or equal to max");

            uint range = (uint)(max - min);
            return ((int)(((ulong)Next() * range) >> 32) + min);
        }

        public static double NextDouble() // generate random double between 0 - 1 (except 1)
        {
            return Next() / 4294967296.0; // üint32 devided by max value of uint32 + 1
        }

        public static double NextDouble(double min, double max) // generate random double between min - max (except max)
        {
            if (min > max)
                throw new ArgumentException("min must be less than or equal to max");

            return (NextDouble() * (max - min)) + min;
        }

        public static bool Chance(float chance) // returns true or false based on probability (0.0 - 1.0)
        {
            return NextDouble() < chance;
        }
    }
}
