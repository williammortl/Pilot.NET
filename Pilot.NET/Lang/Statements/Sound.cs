namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using Pilot.NET.Lang.Expressions.NumericExpressions;
    using Pilot.NET.PILOTExceptions;
    using System;

    /// <summary>
    /// A sound statement, SO
    /// </summary>
    internal sealed class Sound : IImmediateStatement
    {

        /// <summary>
        /// The note to play
        /// </summary>
        private INumericExpression note;

        /// <summary>
        /// The duration to play the sound
        /// </summary>
        private INumericExpression duration;
        
        /// <summary>
        /// Is this a command for a match (M) command?
        /// </summary>
        public MatchTypes MatchType { get; private set; }

        /// <summary>
        /// This is a conditional execution expression
        /// </summary>
        public BooleanCondition IfCondition { get; private set; }

        /// <summary>
        /// The note to play
        /// </summary>
        public INumericExpression Note
        {
            get
            {
                return this.note;
            }
            private set
            {

                // make sure the value isn't null
                if (value == null)
                {
                    throw new InvalidSyntax("Note cannot be a null expression");
                }

                // cannot be an assignment expression
                if (((value is NumericBinaryOperation) == true) && (((NumericBinaryOperation)value).Operator == NumericBinaryOperators.Eq))
                {
                    throw new InvalidSyntax("Note cannot be an assignment expression");
                }

                // assign
                this.note = value;
            }
        }

        /// <summary>
        /// The duration to play the sound
        /// </summary>
        public INumericExpression Duration
        {
            get
            {
                return this.duration;
            }
            private set
            {

                // make sure the value isn't null
                if (value == null)
                {
                    throw new InvalidSyntax("Duration cannot be a null expression");
                }

                // cannot be an assignment expression
                if (((value is NumericBinaryOperation) == true) && (((NumericBinaryOperation)value).Operator == NumericBinaryOperators.Eq))
                {
                    throw new InvalidSyntax("Duration cannot be an assignment expression");
                }

                // assign
                this.duration = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="note">the note to play</param>
        /// <param name="duration">the duration in 1/60th of a second</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifCondition">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public Sound(INumericExpression note, INumericExpression duration, MatchTypes matchType, BooleanCondition ifCondition)
        {
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
            this.Note = note;
            this.Duration = duration;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the compute</returns>
        public override String ToString()
        {
            return StatementMethods.StatementToString(Keywords.SO, this.MatchType, this.IfCondition,
                                                      String.Format("{0}, {1}", this.Note.ToString(), this.Duration.ToString()));
        }
    }
}
