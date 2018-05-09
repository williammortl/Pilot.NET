namespace Pilot.NET
{
    using Pilot.NET.iPilotnterop;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// The state of the PILOT program
    /// </summary>
    public interface IPILOTState
    {

        /// <summary>
        /// A reference to a way for PILOT to interop with the outside world 
        /// </summary>
        IiPilotnterop Interop { get; }

        /// <summary>
        /// List of the name of string variables
        /// </summary>
        List<string> StringVariables { get; }

        /// <summary>
        /// List of the name of numeric variables
        /// </summary>
        List<string> NumericVariables { get; }

        /// <summary>
        /// The turtle's position
        /// </summary>
        Point Turtle { get; }

        /// <summary>
        /// Clears the memory state with respect to variables
        /// </summary>
        void ClearMemoryState();

        /// <summary>
        /// Gets a variable value, can throw RunTimeException
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <returns>the value</returns>
        double GetNumericVar(string varName);

        /// <summary>
        /// Gets a variable value, can throw RunTimeException
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <returns>the value</returns>
        string GetStringVar(string varName);

        /// <summary>
        /// Checks to see if a variable exists
        /// </summary>
        /// <param name="varName">the name of the variable</param>
        /// <returns>true if it exists</returns>
        bool VarExists(string varName);

        /// <summary>
        /// Creates or updates a numeric variable
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <param name="val">the value</param>
        void SetNumericVar(string varName, double val);

        /// <summary>
        /// Creates or updates a string variable
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <param name="val">the value</param>
        void SetStringVar(string varName, string val);
    }
}
