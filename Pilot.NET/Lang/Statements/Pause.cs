namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.PILOTExceptions;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using System;

    /// <summary>
    /// A pause statement, PA
    /// </summary>
    internal sealed class Pause : IStatement
    {

        /// <summary>
        /// The numeric expression for how long to pause, each tick is 1/60 th of a second
        /// </summary>
        private INumericExpression timeToPause;

        /// <summary>
        /// Is this a command for a match (M) command?
        /// </summary>
        public MatchTypes MatchType { get; private set; }

        /// <summary>
        /// This is a conditional execution expression
        /// </summary>
        public BooleanCondition IfCondition { get; private set; }

        /// <summary>
        /// The numeric expression for how long to pause, each tick is 1/60 th of a second
        /// </summary>
        public INumericExpression TimeToPause 
        {
            get
            {
                return this.timeToPause;
            }
            private set
            {

                // make sure the value isn't null
                if (value == null)
                {
                    throw new InvalidSyntax("Cannot pause for null ticks");
                }

                // make sure this is not an assignment expression
                if (((value is NumericBinaryOperation) == true) && (((NumericBinaryOperation)value).Operator == NumericBinaryOperators.Eq))
                {
                    throw new InvalidSyntax("Cannot pause for an assignment expression");
                }

                // assign
                this.timeToPause = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeToPause">numeric expression for how long to pause, each tick is 1/60 th of a second</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public Pause(INumericExpression timeToPause, MatchTypes matchType, BooleanCondition ifCondition)
        {
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
            this.TimeToPause = timeToPause;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the pause time</returns>
        public override String ToString()
        {
            return StatementMethods.StatementToString(Keywords.PA, this.MatchType, this.IfCondition, this.TimeToPause.ToString());
        }
    }
}
