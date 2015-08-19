namespace Pilot.NET.Lang.Expressions.NumericExpressions
{
    using Pilot.NET.PILOTExceptions;
    using Pilot.NET.Lang.Expressions;
    using System;

    /// <summary>
    /// This is a numeric variable
    /// </summary>
    internal sealed class NumericVariable : INumericExpression, IVariable
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
                    throw new InvalidSyntax("Numeric variable must have a name");
                }

                // check for valid variable name
                if ((value.Contains(" ") == true) || (value.Contains("=") == true) || (variableName.StartsWith("#") == false))
                {
                    throw new InvalidSyntax("Not a valid numeric variable name");
                }

                this.variableName = variableName;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="variableName">name</param>
        public NumericVariable(String variableName)
        {
            this.VariableName = variableName;
        }

        /// <summary>
        /// Convert this variable to a string
        /// </summary>
        /// <returns>the string representation</returns>
        public override string ToString()
        {
            return this.VariableName;
        }
    }
}
