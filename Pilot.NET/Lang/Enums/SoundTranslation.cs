namespace Pilot.NET.Lang.Enums
{
    using System.Collections.Generic;

    /// <summary>
    /// Converts sound numbers to frequencies
    /// </summary>
    internal static class SoundTranslation
    {

        /// <summary>
        /// Contains the lookups for the sound# -> frequency conversion
        /// </summary>
        private static Dictionary<int, double> lookup = null;

        /// <summary>
        /// Static constructor
        /// </summary>
        static SoundTranslation()
        {
            SoundTranslation.lookup = new Dictionary<int, double>();
            SoundTranslation.lookup.Add(0, 0);
            SoundTranslation.lookup.Add(1, 130.81);
            SoundTranslation.lookup.Add(2, 138.59);
            SoundTranslation.lookup.Add(3, 146.83);
            SoundTranslation.lookup.Add(4, 155.56);
            SoundTranslation.lookup.Add(5, 164.81);
            SoundTranslation.lookup.Add(6, 174.61);
            SoundTranslation.lookup.Add(7, 185);
            SoundTranslation.lookup.Add(8, 196);
            SoundTranslation.lookup.Add(9, 207.65);
            SoundTranslation.lookup.Add(10, 220);
            SoundTranslation.lookup.Add(11, 233.08);
            SoundTranslation.lookup.Add(12, 246.64);
            SoundTranslation.lookup.Add(13, 261.63);
            SoundTranslation.lookup.Add(14, 0);
            SoundTranslation.lookup.Add(15, 0);
            SoundTranslation.lookup.Add(16, 0);
            SoundTranslation.lookup.Add(17, 0);
            SoundTranslation.lookup.Add(18, 0);
            SoundTranslation.lookup.Add(19, 0);
            SoundTranslation.lookup.Add(20, 0);
            SoundTranslation.lookup.Add(21, 0);
            SoundTranslation.lookup.Add(22, 0);
            SoundTranslation.lookup.Add(23, 0);
            SoundTranslation.lookup.Add(24, 0);
            SoundTranslation.lookup.Add(25, 0);
            SoundTranslation.lookup.Add(26, 0);
            SoundTranslation.lookup.Add(27, 0);
            SoundTranslation.lookup.Add(28, 0);
            SoundTranslation.lookup.Add(29, 0);
            SoundTranslation.lookup.Add(30, 0);
            SoundTranslation.lookup.Add(31, 0);
        }

        /// <summary>
        /// Translates the sound number to a frequency
        /// </summary>
        /// <param name="soundNumber">the # representing the sound</param>
        /// <returns>the frequency</returns>
        public static double Translate(int soundNumber)
        {
            double ret = 0;
            try
            {
                ret = SoundTranslation.lookup[soundNumber] + 100;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
    }
}
