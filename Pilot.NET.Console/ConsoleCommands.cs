namespace Pilot.NET.Console
{
    using System.ComponentModel;

    /// <summary>
    /// The commands for the Pilot.NET console
    /// </summary>
    internal enum ConsoleCommands
    {

        /// <summary>
        /// Quit the Pilot.NET console
        /// </summary>
        [Description("QUITS THE PILOT.NET CONSOLE")]
        QUIT,

        /// <summary>
        /// List the current program in memory
        /// </summary>
        [Description("LISTS THE CURRENT PILOT PROGRAM")]
        LIST,

        /// <summary>
        /// Create a new program, dump any program in memory
        /// </summary>
        [Description("STARTS A NEW PILOT PROGRAM")]
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
        /// List the commands
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
        CLEAR
    }
}
