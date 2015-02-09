namespace Pilot.NET.Lang.Expressions
{
    using Pilot.NET.Lang.Enums;
    using System;

    /// <summary>
    /// Represents a variable
    /// </summary>
    public interface IVariable
    {

        /// <summary>
        /// The name of the variable
        /// </summary>
        String VariableName { get; }

        /// <summary>
        /// The type of variable
        /// </summary>
        VariableTypes TypeOfVariable { get; }
    }
}
