namespace Pilot.NET.Lang.Expressions.StringExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Expressions;
    using System;

    /// <summary>
    /// This is a string variable expression
    /// </summary>
    public sealed class StringVariable : IStringExpression, IVariable
    {

        /// <summary>
        /// The name of the variable
        /// </summary>
        private String variableName;

        /// <summary>
        /// The type of expression
        /// </summary>
        public ExpressionTypes TypeOfExpression { get; private set; }

        /// <summary>
        /// The type of string expression
        /// </summary>
        public StringExpressionTypes TypeOfStringExpression { get; private set; }

        /// <summary>
        /// The type of variable
        /// </summary>
        public VariableTypes TypeOfVariable { get; private set; }

        /// <summary>
        /// The name of the variable
        /// </summary>
        public String VariableName
        {
            get
            {
                return this.variableName;
            }
            private set
            {

                // check to make sure not empty
                String variableName = value.Trim();
                if (String.IsNullOrWhiteSpace(variableName) == true)
                {
                    throw new InvalidSyntax("String variable must have a name");
                }

                // check for valid name
                if ((value.Contains(" ") == true) || (value.Contains("=") == true))
                {
                    throw new InvalidSyntax("Not a valid string variable name");
                }

                // trim the # from the beginning
                if (variableName.StartsWith("$") == true)
                {
                    variableName = variableName.Substring(1);
                }

                this.variableName = variableName;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="variableName">name</param>
        public StringVariable(String variableName)
        {
            this.TypeOfExpression = ExpressionTypes.StringExpression;
            this.TypeOfStringExpression = StringExpressionTypes.StringVariable;
            this.TypeOfVariable = VariableTypes.String;
            this.VariableName = variableName;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">the string variable to duplicate</param>
        public StringVariable(StringVariable toDup)
        {
            this.TypeOfExpression = toDup.TypeOfExpression;
            this.TypeOfStringExpression = toDup.TypeOfStringExpression;
            this.TypeOfVariable = toDup.TypeOfVariable; 
            this.VariableName = toDup.VariableName;
        }

        /// <summary>
        /// Returns a copy
        /// </summary>
        /// <returns>a copy of this PILOT string variable</returns>
        public IExpression Copy()
        {
            return new StringVariable(this);
        }

        /// <summary>
        /// Convert this string variable to a string
        /// </summary>
        /// <returns>the string variable</returns>
        public override string ToString()
        {
            return "$" + this.VariableName;
        }
    }
}
