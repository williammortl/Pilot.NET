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
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">the variable to duplicate</param>
        public RandomNumber(RandomNumber toDup)
        {

            // do nothing! 
        }

        /// <summary>
        /// Returns a copy
        /// </summary>
        /// <returns>a copy of this PILOT random</returns>
        public IExpression Copy()
        {
            return new RandomNumber(this);
        }

        /// <summary>
        /// Convert this variable to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "?";
        }
    }
}
