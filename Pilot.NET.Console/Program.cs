namespace Pilot.NET.Console
{
    using System;
    using Pilot.NET.Lang;
    using Pilot.NET.PILOTParser;
    using Pilot.NET.Interpreter;
    using Pilot.NET.Exception;

    /// <summary>
    /// Entry point for the command line application
    /// </summary>
    class Program
    {

        /// <summary>
        /// Main entry point of the application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {

            // init the console colors
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            // display the banner for Pilot.NET

            // loop in the input loop
            Program.InputLoop();
        }

        private static void InputLoop()
        {

        }
    }
}
