﻿namespace PILOTi
{
    using Pilot.NET;
    using Pilot.NET.Lang;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.PILOTExceptions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Entry point for PILOTi
    /// </summary>
    class Program
    {

        /// <summary>
        /// Usage for PILOTi
        /// </summary>
        private const String PILOT_USAGE = "Usage:\r\n\tPILOTi /? | {name of Pilot file to execute} | {nothing}";

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
        private const String PILOT_PROMPT = "READY";

        /// <summary>
        /// Main entry point of the application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {

            // parse command line arguments
            if (args.Length <= 0)
            {

                // get the current console colors
                ConsoleColor background = Console.BackgroundColor;
                ConsoleColor foreground = Console.ForegroundColor;

                // init the console including console colors 
                Console.Title = Program.PILOT_TITLE;
                Console.WindowLeft = 0;
                Console.WindowTop = 0;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                // display the masthead
                Console.WriteLine(Program.PILOT_MASTHEAD);

                // run the shell
                Program.PilotShell();

                // set the colors back to what they were
                Console.BackgroundColor = background;
                Console.ForegroundColor = foreground;
                Console.Clear();
            }
            else if (args[0].Trim() == "/?")
            {
                Console.WriteLine(Program.PILOT_MASTHEAD);
                Console.WriteLine(Program.PILOT_ABOUT);
                Console.WriteLine(Program.PILOT_USAGE);
            }
            else
            {
                Program.ExecutePilotProgram(args[0].Trim());
            }
        }

        /// <summary>
        /// The Pilot.NET shell
        /// </summary>
        private static void PilotShell()
        {
            using (PILOTInterpreter interpreter = new PILOTInterpreter())
            {

                // var init
                PILOTProgram program = new PILOTProgram();

                // main console command loop
                while (true)
                {

                    // prompt and parse command line
                    String text = Program.Prompt().Trim();
                    if (String.IsNullOrWhiteSpace(text) == false)
                    {

                        // figure out what command text is
                        String[] split = text.Split(new char[1] { ' ' }, 2);
                        split[0] = split[0].ToUpper();
                        if (Enum.IsDefined(typeof(ConsoleCommands), split[0].Trim()) == true)
                        {

                            // console command
                            switch ((ConsoleCommands)Enum.Parse(typeof(ConsoleCommands), split[0].Trim()))
                            {
                                case ConsoleCommands.ABOUT:
                                {
                                    Console.WriteLine();
                                    Console.WriteLine(Program.PILOT_MASTHEAD);
                                    Console.WriteLine(Program.PILOT_ABOUT);
                                    Console.WriteLine(String.Format("PILOT.NET VERSION: {0}", Assembly.GetAssembly(typeof(PILOTProgram)).GetName().Version.ToString()));
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
                                    Console.WriteLine("SHELL COMMANDS:");
                                    Console.WriteLine("---------------");
                                    ConsoleCommands[] commands = (ConsoleCommands[])Enum.GetValues(typeof(ConsoleCommands));
                                    foreach (ConsoleCommands command in commands)
                                    {
                                        FieldInfo fi = command.GetType().GetField(command.ToString());
                                        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                                        Console.WriteLine(String.Format("{0, -10} - {1}", command.ToString(), attributes[0].Description));
                                    }
                                    break;
                                }
                                case ConsoleCommands.NEW:
                                {
                                    program = new PILOTProgram();
                                    break;
                                }
                                case ConsoleCommands.LIST:
                                {
                                    String toPrint = program.ToString().Trim();
                                    if ((split != null) && (split.Length == 2) && (String.IsNullOrWhiteSpace(split[1]) == false))
                                    {
                                        String[] lineStartStop = split[1].Trim().Split(new char[1] { ',' }, 2);
                                        int lineStart = 0;
                                        int lineStop = 0;
                                        if ((lineStartStop.Length >= 2) && (Int32.TryParse(lineStartStop[0], out lineStart) == true) && (Int32.TryParse(lineStartStop[1], out lineStop) == true))
                                        {
                                            toPrint = program.ToString(lineStart, lineStop).Trim();
                                        }
                                        else if (Int32.TryParse(lineStartStop[0], out lineStart) == true)
                                        {
                                            toPrint = program.ToString(lineStart, lineStart).Trim();
                                        }
                                        else
                                        {
                                            toPrint = "INVALID LINE NUMBER(S) TO LIST";
                                        }
                                    }
                                    toPrint = (String.IsNullOrWhiteSpace(toPrint) == true) ? "NO LINES TO LIST" : toPrint;
                                    Console.WriteLine();
                                    Console.WriteLine(toPrint);
                                    break;
                                }
                                case ConsoleCommands.LOAD:
                                {
                                    program = new PILOTProgram();
                                    if ((split != null) && (split.Length == 2) && (String.IsNullOrWhiteSpace(split[1]) == false))
                                    {
                                        try
                                        {
                                            program = PILOTParser.ParseProgram(new FileInfo(split[1].Trim()));
                                        }
                                        catch (PILOTException pe)
                                        {
                                            program = new PILOTProgram();
                                            Console.WriteLine(pe.Message);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("INVALID LOAD COMMAND");
                                    }
                                    break;
                                }
                                case ConsoleCommands.SAVE:
                                {
                                    String progAsString = program.ToString();
                                    if ((split != null) && (split.Length == 2) && (String.IsNullOrWhiteSpace(split[1]) == false))
                                    {
                                        try
                                        {
                                            // create directory if it doesn't exist
                                            String path = Path.GetDirectoryName(split[1]);
                                            if ((String.IsNullOrWhiteSpace(path) == false) && (Directory.Exists(path) == false))
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
                                        Console.WriteLine("INVALID SAVE COMMAND, NEED FILENAME TO SAVE TO");
                                    }
                                    break;
                                }
                                case ConsoleCommands.RUN:
                                {
                                    Console.WriteLine();
                                    try
                                    {
                                        interpreter.Run(program);
                                    }
                                    catch (PILOTException pe)
                                    {
                                        Console.WriteLine(pe.Message);
                                    }
                                    break;
                                }
                                case ConsoleCommands.DIR:
                                {
                                    Console.WriteLine();
                                    String message = String.Format("CONTENTS OF: {0}", Environment.CurrentDirectory).ToUpper();
                                    Console.WriteLine(message);
                                    Console.WriteLine(new String('-', message.Length));
                                    foreach (String dir in Directory.GetDirectories(Environment.CurrentDirectory))
                                    {
                                        Console.WriteLine(String.Format("[{0}]", Path.GetFileName(dir)));
                                    }
                                    foreach (String file in Directory.GetFiles(Environment.CurrentDirectory))
                                    {
                                        Console.WriteLine(String.Format("{0}", Path.GetFileName(file)));
                                    }
                                    break;
                                }
                                case ConsoleCommands.CD:
                                {
                                    Console.WriteLine();
                                    try
                                    {
                                        Environment.CurrentDirectory = split[1].Trim();
                                        Console.WriteLine(String.Format("DIRECTORY CHANGED TO: {0}", Environment.CurrentDirectory));
                                    }
                                    catch
                                    {
                                        Console.WriteLine("INVALID DIRECTORY");
                                    }
                                    break;
                                }
                                case ConsoleCommands.DEL:
                                {
                                    Console.WriteLine();
                                    try
                                    {
                                        String fileToDelete = split[1].Trim();
                                        if (File.Exists(fileToDelete) == true)
                                        {
                                            File.Delete(fileToDelete);
                                            Console.WriteLine(String.Format("{0} WAS SUCCESSFULLY DELETED", fileToDelete));
                                        }
                                        else
                                        {
                                            throw new FileNotFoundException();
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("COULD NOT DELETE FILE");
                                    }
                                    break;
                                }
                                case ConsoleCommands.TYPE:
                                {
                                    Console.WriteLine();
                                    try
                                    {
                                        String fileToDisplay = split[1].Trim();
                                        String[] fileContents = File.ReadAllLines(fileToDisplay);
                                        foreach (String line in fileContents)
                                        {
                                            Console.WriteLine(line);
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("COULD NOT DISPLAY FILE");
                                    }
                                    break;
                                }
                                case ConsoleCommands.KEYWORDS:
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("PILOT.NET KEYWORDS:");
                                    Console.WriteLine("-------------------");
                                    Keywords[] keywords = (Keywords[])Enum.GetValues(typeof(Keywords));
                                    foreach (Keywords keyword in keywords)
                                    {
                                        FieldInfo fi = keyword.GetType().GetField(keyword.ToString());
                                        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                                        Console.WriteLine(String.Format("{0, -10} - {1}", keyword.ToString(), attributes[0].Description));
                                    }
                                    break;
                                }
                                case ConsoleCommands.VARS:
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("CURRENT VARS:");
                                    Console.WriteLine("-------------");
                                    if ((interpreter.StringVariables.Count > 0) || (interpreter.NumericVariables.Count > 0))
                                    {
                                        foreach (String varName in interpreter.StringVariables)
                                        {
                                            Console.WriteLine(String.Format("{0, -10} = {1}", varName, interpreter.GetStringVar(varName)));
                                        }
                                        foreach (String varName in interpreter.NumericVariables)
                                        {
                                            Console.WriteLine(String.Format("{0, -10} = {1}", varName, interpreter.GetNumericVar(varName).ToString("0.00")));
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("NO VARIABLES TO DISPLAY");
                                    }
                                    break;
                                }
                                case ConsoleCommands.REN:
                                {
                                    List<int> lineNumbersOrig = program.LineNumbers;
                                    if ((lineNumbersOrig != null) && (lineNumbersOrig.Count > 0))
                                    {

                                        // figure out start and incrememnt
                                        int start = 10;
                                        int increment = 10;
                                        if ((split != null) && (split.Length == 2) && (String.IsNullOrWhiteSpace(split[1]) == false))
                                        {
                                            String[] renumStartIncrement = split[1].Trim().Split(new char[1] { ',' }, 2);
                                            if ((renumStartIncrement.Length >= 1) && (Program.IsInt(renumStartIncrement[0]) == true))
                                            {
                                                start = Convert.ToInt32(renumStartIncrement[0]);
                                            }
                                            if ((renumStartIncrement.Length == 2) && (Program.IsInt(renumStartIncrement[1]) == true))
                                            {
                                                increment = Convert.ToInt32(renumStartIncrement[1]);
                                            }
                                        }

                                        // actually renumber
                                        PILOTProgram newProgram = new PILOTProgram();
                                        int currentValue = start;
                                        int totalLines = program.LineNumbers.Count;
                                        for (int i = 0; i < totalLines; i++)
                                        {
                                            newProgram[currentValue] = program[lineNumbersOrig[i]];
                                            currentValue = currentValue + increment;
                                        }
                                        program = newProgram;
                                    }
                                    break;
                                }
                                case ConsoleCommands.VERSION:
                                {
                                    Console.WriteLine();
                                    Console.WriteLine(String.Format("PILOT.NET VERSION: {0}", Assembly.GetAssembly(typeof(PILOTProgram)).GetName().Version.ToString()));
                                    break;
                                }
                                case ConsoleCommands.OPERATORS:
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("BOOLEAN OPERATORS:");
                                    Console.WriteLine("------------------");
                                    BooleanConditionOperators[] boolOperators = (BooleanConditionOperators[])Enum.GetValues(typeof(BooleanConditionOperators));
                                    foreach (BooleanConditionOperators op in boolOperators)
                                    {
                                        FieldInfo fi = op.GetType().GetField(op.ToString());
                                        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                                        Console.WriteLine(attributes[0].Description);
                                    }
                                    Console.WriteLine();
                                    Console.WriteLine("NUMERIC OPERATORS:");
                                    Console.WriteLine("------------------");
                                    NumericBinaryOperators[] numericOperators = (NumericBinaryOperators[])Enum.GetValues(typeof(NumericBinaryOperators));
                                    foreach (NumericBinaryOperators op in numericOperators)
                                    {
                                        FieldInfo fi = op.GetType().GetField(op.ToString());
                                        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                                        Console.WriteLine(attributes[0].Description);
                                    }
                                    break;
                                }
                                case ConsoleCommands.COLORS:
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("COLORS:");
                                    Console.WriteLine("-------");
                                    PenColors[] colors = (PenColors[])Enum.GetValues(typeof(PenColors));
                                    foreach (PenColors color in colors)
                                    {
                                        Console.WriteLine(String.Format("{0, -10} - {1}", color.ToString(), ((int)color).ToString()));
                                    }
                                    break;
                                }
                                case ConsoleCommands.TURTLE:
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("TURTLE EXPRESSIONS:");
                                    Console.WriteLine("-------------------");
                                    GraphicsExpressionKeywords[] graphExp = (GraphicsExpressionKeywords[])Enum.GetValues(typeof(GraphicsExpressionKeywords));
                                    foreach (GraphicsExpressionKeywords op in graphExp)
                                    {
                                        FieldInfo fi = op.GetType().GetField(op.ToString());
                                        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                                        Console.WriteLine(String.Format("{0, -10} - {1}", op.ToString(), attributes[0].Description));
                                    }
                                    break;
                                }
                            }
                        }
                        else if (Program.IsInt(split[0].Trim()) == true)
                        {
                            if ((split.Length < 2) || (String.IsNullOrWhiteSpace(split[1]) == true))
                            {

                                // delete the line
                                program[Convert.ToInt32(split[0])] = null;
                            }
                            else
                            {
                                try
                                {

                                    // parse the line and add / replace
                                    Line line = PILOTParser.ParseLine(split[1]);
                                    program[Convert.ToInt32(split[0])] = line;
                                }
                                catch (PILOTException pe)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine(pe.Message);
                                }
                            }
                        }
                        else
                        {

                            // evaluate immediate statement
                            try
                            {
                                interpreter.EvaluateImmediateStatement(text);
                            }
                            catch (PILOTException pe)
                            {
                                Console.WriteLine();
                                Console.WriteLine(pe.Message);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads and executes a Pilot program from a file
        /// </summary>
        /// <param name="pilotFile">the Pilot program to execute</param>
        private static void ExecutePilotProgram(String pilotFile)
        {
            if (File.Exists(pilotFile) == true)
            {
                PILOTInterpreter interpreter = new PILOTInterpreter();
                interpreter.Run(new FileInfo(pilotFile));
            }
            else
            {
                Console.WriteLine("File not found!");
                Console.WriteLine();
                Console.WriteLine(Program.PILOT_MASTHEAD);
                Console.WriteLine(Program.PILOT_ABOUT);
                Console.WriteLine(Program.PILOT_USAGE);
            }
        }

        /// <summary>
        /// Displays the PILOT.NET prompt and retrieves input
        /// </summary>
        /// <returns>the input</returns>
        private static String Prompt()
        {
            Console.WriteLine();
            Console.WriteLine(Program.PILOT_PROMPT);
            return Console.ReadLine().Trim();
        }

        /// <summary>
        /// Is the string an int?
        /// </summary>
        /// <param name="str">the string to check</param>
        /// <returns>true if it is an int</returns>
        private static Boolean IsInt(String str)
        {
            int lineNumber = 0;
            return Int32.TryParse(str.Trim(), out lineNumber);
        }
    }
}
