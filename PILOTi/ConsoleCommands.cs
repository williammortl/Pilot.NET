namespace PILOTi
{
    using System.ComponentModel;

    /// <summary>
    /// The commands for Pilot.NET
    /// </summary>
    internal enum ConsoleCommands
    {

        /// <summary>
        /// Quits Pilot.NET
        /// </summary>
        [Description("QUITS PILOT.NET")]
        QUIT,

        /// <summary>
        /// List the current program in memory
        /// </summary>
        [Description("LISTS THE CURRENT PILOT PROGRAM")]
        LIST,

        /// <summary>
        /// Create a new program, dump any program in memory
        /// </summary>
        [Description("CREATES A NEW PILOT PROGRAM")]
        NEW,

        /// <summary>
        /// Load a program from file
        /// </summary>
        [Description("LOADS A PROGRAM FROM A FILE")]
        LOAD,

        /// <summary>
        /// Save a program to a file
        /// </summary>
        [Description("SAVES THE CURRENT PROGRAM TO FILE")]
        SAVE,

        /// <summary>
        /// List PILOT.NET commands
        /// </summary>
        [Description("LISTS PILOT.NET COMMANDS")]
        HELP,

        /// <summary>
        /// About Pilot.NET
        /// </summary>
        [Description("ABOUT PILOT.NET")]
        ABOUT,

        /// <summary>
        /// Clear the screen
        /// </summary>
        [Description("CLEAR THE SCREEN")]
        CLEAR,

        /// <summary>
        /// Runs the program in memory
        /// </summary>
        [Description("RUNS THE PROGRAM CURRENTLY IN MEMORY")]
        RUN    
    }
}
