namespace Pilot.NET.Lang.Expressions.NumericExpressions
{
    using Pilot.NET.Lang.Expressions;

    /// <summary>
    /// This is a random number
    /// </summary>
    internal sealed class RandomNumber : INumericExpression
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public RandomNumber()
        {

            // do nothing! 
        }

        /// <summary>
        /// Convert this variable to a string
        /// </summary>
        /// <returns>the string representation</returns>
        public override string ToString()
        {
            return "?";
        }
    }
}
