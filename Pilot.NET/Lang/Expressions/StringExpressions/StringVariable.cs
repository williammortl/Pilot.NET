namespace Pilot.NET.Lang.Expressions.StringExpressions
{
    using Pilot.NET.Exception;
    using Pilot.NET.Lang.Expressions;
    using System;

    /// <summary>
    /// This is a string variable expression
    /// </summary>
    internal sealed class StringVariable : IStringExpression, IVariable
    {

        /// <summary>
        /// The name of the variable
        /// </summary>
        private String variableName;

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
            this.VariableName = variableName;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">the string variable to duplicate</param>
        public StringVariable(StringVariable toDup)
        {
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
