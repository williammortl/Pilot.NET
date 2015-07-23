namespace Pilot.NET.Lang.Expressions.NumericExpressions
{
    using Pilot.NET.Lang.Expressions;

    /// <summary>
    /// This is a numeric literal
    /// </summary>
    internal sealed class NumericLiteral : INumericExpression
    {

        /// <summary>
        /// The name of the variable
        /// </summary>
        public double Number { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="number">the numeric literal</param>
        public NumericLiteral(double number)
        {
            this.Number = number;
        }

        /// <summary>
        /// Convert this variable to a string
        /// </summary>
        /// <returns>the string representation</returns>
        public override string ToString()
        {
            return this.Number.ToString();
        }
    }
}
