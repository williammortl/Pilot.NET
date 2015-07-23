namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Exception;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.Lang.Expressions.StringExpressions;
    using System;

    /// <summary>
    /// A compute statement, C
    /// </summary>
    internal sealed class Compute : IImmediateStatement
    {

        /// <summary>
        /// The expression to compute
        /// </summary>
        private IExpression expressionToCompute;

        /// <summary>
        /// The type of command
        /// </summary>
        public Keywords Keyword { get; private set; }

        /// <summary>
        /// Is this a command for a match (M) command?
        /// </summary>
        public MatchTypes MatchType { get; private set; }

        /// <summary>
        /// This is a conditional execution expression
        /// </summary>
        public BooleanCondition IfCondition { get; private set; }

        /// <summary>
        /// The expression to compute
        /// </summary>
        public IExpression ExpressionToCompute 
        {
            get
            {
                return this.expressionToCompute;
            }
            private set
            {

                // make sure the value isn't null
                if (value == null)
                {
                    throw new InvalidSyntax("Cannot perform a compute operation on a null expression");
                }

                // check to make sure that the expression isnt null, and is an assignment expression
                if (((value is StringAssignExpression) == false) && ((value is NumericBinaryOperation) == false))
                {
                    throw new InvalidSyntax("Compute must have a expression to compute");
                }

                // assign
                this.expressionToCompute = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expressionToCompute">the expression to compute</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public Compute(IExpression expressionToCompute, MatchTypes matchType, BooleanCondition ifCondition)
        {
            this.Keyword = Keywords.C;
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
            this.ExpressionToCompute = expressionToCompute;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the compute</returns>
        public override String ToString()
        {
            return StatementMethods.StatementToString(this.Keyword, this.MatchType, this.IfCondition, this.ExpressionToCompute.ToString());
        }
    }
}
