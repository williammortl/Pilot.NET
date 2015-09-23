namespace PILOTi
{
    using System.ComponentModel;

    /// <summary>
    /// The commands for Pilot.NET
    /// </summary>
    internal enum ConsoleCommands
    {

        /// <summary>
        /// About Pilot.NET
        /// </summary>
        [Description("ABOUT PILOT.NET")]
        ABOUT,

        /// <summary>
        /// Changes directory
        /// </summary>
        [Description("CHANGES DIRECTORY")]
        CD,

        /// <summary>
        /// Clear the screen
        /// </summary>
        [Description("CLEARS THE SCREEN")]
        CLEAR,

        /// <summary>
        /// Lists the colors
        /// </summary>
        [Description("PILOT.NET COLORS")]
        COLORS,

        /// <summary>
        /// Lists files in the directory
        /// </summary>
        [Description("DELETES A FILE")]
        DEL,

        /// <summary>
        /// Lists files in the directory
        /// </summary>
        [Description("LISTS FILES IN THE DIRECTORY")]
        DIR,

        /// <summary>
        /// List PILOT.NET commands
        /// </summary>
        [Description("LISTS SHELL COMMANDS")]
        HELP,

        /// <summary>
        /// List PILOT.NET keywords
        /// </summary>
        [Description("LISTS KEYWORDS")]
        KEYWORDS,

        /// <summary>
        /// List the current program in memory
        /// </summary>
        [Description("LISTS THE CURRENT PILOT PROGRAM")]
        LIST,

        /// <summary>
        /// Load a program from file
        /// </summary>
        [Description("LOADS A PROGRAM FROM A FILE")]
        LOAD,

        /// <summary>
        /// Create a new program, dump any program in memory
        /// </summary>
        [Description("CREATES A NEW PILOT PROGRAM")]
        NEW,

        /// <summary>
        /// Lists operators
        /// </summary>
        [Description("LISTS PILOT.NET OPERATORS")]
        OPERATORS,
        
        /// <summary>
        /// Quits Pilot.NET
        /// </summary>
        [Description("QUITS THE PILOT.NET SHELL")]
        QUIT,

        /// <summary>
        /// Renumbers a program
        /// </summary>
        [Description("RENUMBERS A PROGRAM")]
        REN,

        /// <summary>
        /// Runs the program in memory
        /// </summary>
        [Description("RUNS THE PROGRAM CURRENTLY IN MEMORY")]
        RUN,

        /// <summary>
        /// Save a program to a file
        /// </summary>
        [Description("SAVES THE CURRENT PROGRAM TO FILE")]
        SAVE,
        
        /// <summary>
        /// Save a program to a file
        /// </summary>
        [Description("DISPLAYS A FILE'S CONTENTS")]
        TYPE,

        /// <summary>
        /// Print all variables
        /// </summary>
        [Description("PRINTS ALL VARIABLES IN MEMORY")]
        VARS,

        /// <summary>
        /// Print all variables
        /// </summary>
        [Description("PRINTS THE PILOT VERSION")]
        VERSION
    }
}
