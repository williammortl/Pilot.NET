namespace Pilot.NET.Lang.Statements
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Expressions.Boolean;
    using System;

    /// <summary>
    /// A remark statement, R
    /// </summary>
    public sealed class Remark : IStatement
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
        /// The text of the comment
        /// </summary>
        public String Comment { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comment">the parameters of the remark statement, which is the comment text</param>
        public Remark(String comment)
        {
            this.Keyword = Keywords.R;
            this.MatchType = MatchTypes.None;
            this.IfCondition = null;
            this.Comment = comment;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">to duplicate</param>
        public Remark(Remark toDup)
        {
            this.Keyword = toDup.Keyword;
            this.MatchType = MatchTypes.None;
            this.IfCondition = null;
            this.Comment = toDup.Comment;
        }

        /// <summary>
        /// Returns a copy of the remark
        /// </summary>
        /// <returns>the copy</returns>
        public IStatement Copy()
        {
            return new Remark(this);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string representation of the remark</returns>
        public override String ToString()
        {
            return StatementMethods.StatementToString(this.Keyword, this.MatchType, this.IfCondition, this.Comment);
        }
    }
}
