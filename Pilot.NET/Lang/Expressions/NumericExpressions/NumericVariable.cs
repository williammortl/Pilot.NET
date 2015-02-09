namespace Pilot.NET.Lang.Expressions.NumericExpressions
{
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.Lang.Exceptions;
    using Pilot.NET.Lang.Expressions;
    using System;

    /// <summary>
    /// This is a numeric variable
    /// </summary>
    public sealed class NumericVariable : INumericExpression, IVariable
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
        /// The type of numeric expression
        /// </summary>
        public NumericExpressionTypes TypeOfNumericExpression { get; private set; }

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
                    throw new InvalidSyntax("Numeric variable must have a name");
                }

                // check for valid variable name
                if ((value.Contains(" ") == true) || (value.Contains("=") == true))
                {
                    throw new InvalidSyntax("Not a valid numeric variable name");
                }

                // trim the # from the beginning
                if (variableName.StartsWith("#") == true)
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
        public NumericVariable(String variableName)
        {
            this.TypeOfExpression = ExpressionTypes.NumericExpression;
            this.TypeOfNumericExpression = NumericExpressionTypes.NumericVariable;
            this.TypeOfVariable = VariableTypes.Numeric;
            this.VariableName = variableName;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toDup">the variable to duplicate</param>
        public NumericVariable(NumericVariable toDup)
        {
            this.TypeOfExpression = toDup.TypeOfExpression;
            this.TypeOfNumericExpression = toDup.TypeOfNumericExpression;
            this.TypeOfVariable = toDup.TypeOfVariable;
            this.VariableName = toDup.VariableName;
        }

        /// <summary>
        /// Returns a copy
        /// </summary>
        /// <returns>a copy of this PILOT variable</returns>
        public IExpression Copy()
        {
            return new NumericVariable(this);
        }

        /// <summary>
        /// Convert this variable to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "#" + this.VariableName;
        }
    }
}
