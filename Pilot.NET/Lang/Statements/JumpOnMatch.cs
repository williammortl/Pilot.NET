namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Expressions.Boolean;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A jump on match statement, JM
    /// </summary>
    public sealed class JumpOnMatch : IStatement
    {

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
        /// The labels to conditionally jump to
        /// </summary>
        public LinkedList<Label> LabelsToJumpTo { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="labelsToJumpTo">the labels to jump to</param>
        /// <param name="matchType">the match type</param>
        /// <param name="ifExpression">a boolean expression, if it evaluates to true then execute the statement, can be null</param>
        public JumpOnMatch(LinkedList<Label> labelsToJumpTo, MatchTypes matchType, BooleanCondition ifCondition)
        {

            // init attributes
            this.Keyword = Keywords.JM;
            this.MatchType = matchType;
            this.IfCondition = ifCondition;
            this.LabelsToJumpTo = labelsToJumpTo;
                        
            // verify at least 1 label is present
            if ((this.LabelsToJumpTo == null) || (this.LabelsToJumpTo.Count < 1))
            {
                throw new InvalidSyntax("Jump on match needs to have at least 1 label to jump to");
            }
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">to duplicate</param>
        public JumpOnMatch(JumpOnMatch toDup)
        {

            // deep copy
            this.Keyword = toDup.Keyword;
            this.MatchType = toDup.MatchType;
            this.IfCondition = (BooleanCondition)((toDup.IfCondition == null) ? null : toDup.IfCondition.Copy());
            this.LabelsToJumpTo = new LinkedList<Label>();

            // verify at least 1 label is present
            if ((toDup.LabelsToJumpTo == null) || (toDup.LabelsToJumpTo.Count < 1))
            {
                throw new InvalidSyntax("Jump on match needs to have at least 1 label to jump to");
            }

            // deep copy each label to jump to
            foreach(Label LabelToJumpTo in toDup.LabelsToJumpTo)
            {
                this.LabelsToJumpTo.AddLast(LabelToJumpTo.Copy());
            }
        }

        /// <summary>
        /// Returns a copy of the jump on match
        /// </summary>
        /// <returns>the copy</returns>
        public IStatement Copy()
        {
            return new JumpOnMatch(this);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the jump on match</returns>
        public override String ToString()
        {

            // loop through labels
            String labelsString = String.Empty;
            foreach (Label label in this.LabelsToJumpTo)
            {
                labelsString += " " + label.ToString() + ",";
            }
            labelsString = labelsString.Substring(0, labelsString.Length - 1);

            return StatementMethods.StatementToString(this.Keyword, this.MatchType, this.IfCondition, labelsString);
        }
    }
}
