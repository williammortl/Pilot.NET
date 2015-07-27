namespace Pilot.NET.Console
{
    using System;
    using Pilot.NET.Interpreter;
    using Pilot.NET.PILOTParser;
    using Pilot.NET.Lang;
    using System.Reflection;
    using System.ComponentModel;
    using System.IO;
    using Pilot.NET.Exception;

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
        private const String PILOT_MASTHEAD = "PILOT.NET (c) COPYRIGHT WILLIAM MORTL 2015";

        /// <summary>
        /// Pilot.NET about
        /// </summary>
        private const String PILOT_ABOUT = "LOVINGLY DEDICATED TO ATARI PILOT & JOHN AMSDEN STARKWEATHER\r\nBY WILLIAM MICHAEL MORTL - HTTP://WWW.WILLIAMMORTL.COM";

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

                // try to convert to console command
                if (Enum.IsDefined(typeof(ConsoleCommands), split[0].Trim()) == true)
                {

                    // convert to console command
                    ConsoleCommands cmd = (ConsoleCommands)Enum.Parse(typeof(ConsoleCommands), split[0].Trim());

                    // execute the command
                    switch (cmd)
                    {
                        case ConsoleCommands.ABOUT:
                        {
                            Console.WriteLine();
                            Console.WriteLine(Program.PILOT_MASTHEAD);
                            Console.WriteLine(Program.PILOT_ABOUT);
                            break;
                        }
                        case ConsoleCommands.CLEAR:
                        {
                            Console.Clear();
                            break;
                        }
                        case ConsoleCommands.QUIT:
                        {
                            return;
                        }
                        case ConsoleCommands.HELP:
                        {
                            Console.WriteLine();
                            Console.WriteLine("Console Commands:");
                            Console.WriteLine("-----------------");
                            ConsoleCommands[] commands = (ConsoleCommands[])Enum.GetValues(typeof(ConsoleCommands));
                            foreach(ConsoleCommands command in commands)
                            {
                                FieldInfo fi = command.GetType().GetField(command.ToString());
                                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                                Console.WriteLine(String.Format("{0}\t- {1}", command.ToString(), attributes[0].Description));
                            }
                            break;
                        }
                        case ConsoleCommands.NEW:
                        {
                            prog = new PILOTProgram();
                            break;
                        }
                        case ConsoleCommands.LIST:
                        {
                            String progText = prog.ToString().Trim();
                            if (String.IsNullOrWhiteSpace(progText) == false)
                            {
                                Console.WriteLine();
                                Console.WriteLine(progText);
                            }
                            break;
                        }
                        case ConsoleCommands.LOAD:
                        {
                            if ((split != null) && (split.Length == 2) && (String.IsNullOrWhiteSpace(split[1]) == false))
                            {
                                try
                                {
                                    prog = Parser.ParseProgram(new FileInfo(split[1]));
                                }
                                catch (PILOTException pe)
                                {
                                    prog = new PILOTProgram();
                                    Console.WriteLine(pe.Message);
                                }
                            }
                            else
                            {
                                Console.WriteLine("INVALID LOAD COMMAND");
                            }
                            break;
                        }
                        case ConsoleCommands.SAVE:
                        {
                            String progAsString = prog.ToString();
                            if (String.IsNullOrWhiteSpace(progAsString) == true)
                            {
                                Console.WriteLine();
                                Console.WriteLine("NOTHING TO SAVE");
                            }
                            else if ((split != null) && (split.Length == 2) && (String.IsNullOrWhiteSpace(split[1]) == false))
                            {
                                try
                                {
                                    // create directory if it doesn't exist
                                    String path = Path.GetDirectoryName(split[1]);
                                    if (Directory.Exists(path) == false)
                                    {
                                        Directory.CreateDirectory(path);
                                    }

                                    // write the program to a file
                                    using (StreamWriter sw = new StreamWriter(split[1]))
                                    {
                                        sw.Write(progAsString);
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("ERROR OCCURRED WHILE SAVING");
                                }
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("INVALID SAVE COMMAND");
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
