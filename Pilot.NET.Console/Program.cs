namespace Pilot.NET.Console
{
    using System;
    using Pilot.NET.Interpreter;
    using Pilot.NET.PILOTParser;
    using Pilot.NET.Lang;
    using System.Reflection;
    using System.ComponentModel;

    /// <summary>
    /// Entry point for the command line application
    /// </summary>
    class Program
    {

        /// <summary>
        /// The title for the Pilot.NET console window
        /// </summary>
        private const String PILOT_TITLE = "Pilot.NET";

        /// <summary>
        /// Pilot.NET masthead
        /// </summary>
        private const String PILOT_MASTHEAD = "PILOT.NET (c) COPYRIGHT WILLIAM MORTL 2015\r\nLOVINGLY DEDICATED TO ATARI PILOT & JOHN AMSDEN STARKWEATHER\r\nBY WILLIAM MICHAEL MORTL - HTTP://WWW.WILLIAMMORTL.COM";

        /// <summary>
        /// Pilot.NET prompt
        /// </summary>
        private const String PILOT_PROMPT = "\r\nREADY";

        /// <summary>
        /// Main entry point of the application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {

            // init the console colors, display the masthead
            Console.Title = Program.PILOT_TITLE;
            Console.WindowLeft = 0;
            Console.WindowTop = 0;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine(Program.PILOT_MASTHEAD);

            // var init
            PILOTInterpreter pi = new PILOTInterpreter();
            PILOTProgram prog = new PILOTProgram();

            // main console command loop
            while (true)
            {

                // prompt and get input
                String text = Program.Prompt();
                String[] split = text.ToUpper().Split(new char[1] { ' ' });

                // try to convert to command
                if (Enum.IsDefined(typeof(ConsoleCommands), split[0].Trim()) == true)
                {

                    // convert
                    ConsoleCommands cmd = (ConsoleCommands)Enum.Parse(typeof(ConsoleCommands), split[0].Trim());

                    // execute the command
                    switch (cmd)
                    {
                        case ConsoleCommands.ABOUT:
                        {
                            Console.WriteLine();
                            Console.WriteLine(Program.PILOT_MASTHEAD);
                            break;
                        }
                        case ConsoleCommands.QUIT:
                        {
                            return;
                        }
                        case ConsoleCommands.HELP:
                        {
                            Console.WriteLine();
                            ConsoleCommands[] commands = (ConsoleCommands[])Enum.GetValues(typeof(ConsoleCommands));
                            foreach(ConsoleCommands command in commands)
                            {
                                FieldInfo fi = command.GetType().GetField(command.ToString());
                                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                                Console.WriteLine(String.Format("{0} - {1}", command.ToString(), attributes[0].Description));
                            }
                            break;
                        }
                    }
                }
                else
                {

                    
                }
            }
        }

        /// <summary>
        /// Displays the PILOT.NET prompt and retrieves input
        /// </summary>
        /// <returns>the input</returns>
        private static String Prompt()
        {
            Console.WriteLine(Program.PILOT_PROMPT);
            return Console.ReadLine().Trim();
        }
    }
}
