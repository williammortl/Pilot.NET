namespace Pilot.NET
{
    using System;

    /// <summary>
    /// Util methods for PILOT
    /// </summary>
    internal static class Util
    {

        /// <summary>
        /// For the random number generator
        /// </summary>
        private static Random randomGenerator = new Random();

        /// <summary>
        /// Random number generator
        /// </summary>
        public static Random RandomGenerator()
        {
            return Util.randomGenerator;
        }
    }
}
