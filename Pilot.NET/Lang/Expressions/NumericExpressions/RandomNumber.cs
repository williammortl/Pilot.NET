namespace Pilot.NET.Lang.Expressions.NumericExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions;

    /// <summary>
    /// This is a random number
    /// </summary>
    public sealed class RandomNumber : INumericExpression
    {

        /// <summary>
        /// The type of expression
        /// </summary>
        public ExpressionTypes TypeOfExpression { get; private set; }

        /// <summary>
        /// The type of numeric expression
        /// </summary>
        public NumericExpressionTypes TypeOfNumericExpression { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RandomNumber()
        {
            this.TypeOfExpression = ExpressionTypes.NumericExpression;
            this.TypeOfNumericExpression = NumericExpressionTypes.RandomNumber;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">the variable to duplicate</param>
        public RandomNumber(RandomNumber toDup)
        {
            this.TypeOfExpression = toDup.TypeOfExpression;
            this.TypeOfNumericExpression = toDup.TypeOfNumericExpression;
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
