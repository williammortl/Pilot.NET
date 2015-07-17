namespace Pilot.NET.Lang.Expressions
{
    using System;

    /// <summary>
    /// Represents a variable
    /// </summary>
    internal interface IVariable
    {

        /// <summary>
        /// The name of the variable
        /// </summary>
        String VariableName { get; }
    }
}
