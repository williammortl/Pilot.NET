namespace Pilot.NET.Lang.Expressions.NumericExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions;

    /// <summary>
    /// This is a numeric literal
    /// </summary>
    public sealed class NumericLiteral : INumericExpression
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
        /// The name of the variable
        /// </summary>
        public double Number { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="number">the numeric literal</param>
        public NumericLiteral(double number)
        {
            this.TypeOfExpression = ExpressionTypes.NumericExpression;
            this.TypeOfNumericExpression = NumericExpressionTypes.NumericVariable;
            this.Number = number;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">the variable to duplicate</param>
        public NumericLiteral(NumericLiteral toDup)
        {
            this.TypeOfExpression = toDup.TypeOfExpression;
            this.TypeOfNumericExpression = toDup.TypeOfNumericExpression;
            this.Number = toDup.Number;
        }

        /// <summary>
        /// Returns a copy
        /// </summary>
        /// <returns>a copy of this PILOT literal</returns>
        public IExpression Copy()
        {
            return new NumericLiteral(this);
        }

        /// <summary>
        /// Convert this variable to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Number.ToString();
        }
    }
}
