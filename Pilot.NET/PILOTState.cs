namespace Pilot.NET
{
    using Pilot.NET.ExternalInterfaces;
    using Pilot.NET.Lang.Enums;
    using Pilot.NET.PILOTExceptions;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// The state for a PILOT program
    /// </summary>
    internal sealed class PILOTState
    {

        /// <summary>
        /// turtle graphics x coordinate
        /// </summary>
        private const String X_VAR = "%X";

        /// <summary>
        /// turtle graphics y coordinate
        /// </summary>
        private const String Y_VAR = "%Y";

        /// <summary>
        /// turtle graphics theta coordinate
        /// </summary>
        private const String THETA_VAR = "%A";

        /// <summary>
        /// turtle graphics color value
        /// </summary>
        private const String COLOR_VAR = "%Z";

        /// <summary>
        /// turtle graphics pen width value
        /// </summary>
        private const String WIDTH_VAR = "%W";

        /// <summary>
        /// For the random number generator
        /// </summary>
        private static Random randomGenerator = new Random();

        /// <summary>
        /// String variables and their associated values
        /// </summary>
        private Dictionary<String, String> stringVariables;

        /// <summary>
        /// Numeric variables and their associated values
        /// </summary>
        private Dictionary<String, double> numericVariables;

        /// <summary>
        /// A reference to an external interface for PILOT to interact with 
        /// with the program
        /// </summary>
        public IPILOTExternalInterface ExternalInterface { get; private set; }

        /// <summary>
        /// List of the name of string variables
        /// </summary>
        public List<string> StringVariables
        {
            get
            {
                return this.stringVariables.Keys.ToList<string>();
            }
        }

        /// <summary>
        /// List of the name of numeric variables
        /// </summary>
        public List<string> NumericVariables
        {
            get
            {
                return this.numericVariables.Keys.ToList<string>();
            }
        }

        /// <summary>
        /// The turtle's position
        /// </summary>
        public Point TurtlePosition { get; private set; }

        /// <summary>
        /// Random number generator
        /// </summary>
        public Random RandomGenerator
        {
            get
            {
                return PILOTState.randomGenerator;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="externalInterface">external interface to use</param>
        public PILOTState(IPILOTExternalInterface externalInterface)
        {
            // check to make sure that interface is not null
            if (externalInterface == null)
            {
                throw new PILOTException("Must provide a valid external interface for the PILOT state");
            }

            // create objects
            this.ExternalInterface = externalInterface;
            this.stringVariables = new Dictionary<string, string>();
            this.numericVariables = new Dictionary<string, double>();
            this.TurtlePosition = new Point(0, 0);

            // clear / init the memory state
            this.ClearMemoryState();
        }

        /// <summary>
        /// Clears the memory state with respect to variables
        /// </summary>
        public void ClearMemoryState()
        {
            // reset vars
            this.stringVariables.Clear();
            this.numericVariables.Clear();

            // turtle graphics init
            this.TurtlePosition = new Point(0, 0);
            this.SetNumericVar(PILOTState.X_VAR, 0);
            this.SetNumericVar(PILOTState.Y_VAR, 0);
            this.SetNumericVar(PILOTState.THETA_VAR, 0);
            this.SetNumericVar(PILOTState.WIDTH_VAR, 1);
            this.SetNumericVar(PILOTState.COLOR_VAR, (double)PenColors.YELLOW);
        }

        /// <summary>
        /// Gets a variable value, can throw RunTimeException
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <returns>the value</returns>
        public double GetNumericVar(string varName)
        { 
            // var init
            double retVal = 0;

            // look for variable
            varName = varName.Trim().ToUpper();
            if (this.numericVariables.Keys.Contains(varName) == true)
            {
                retVal = this.numericVariables[varName];
            }
            else
            {
                throw new RunTimeException(String.Format("Could not find numeric variable: {0}", varName));
            }

            return retVal;
        }

        /// <summary>
        /// Gets a variable value, can throw RunTimeException
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <returns>the value</returns>
        public string GetStringVar(string varName)
        {
            // var init
            String retVal = String.Empty;

            // look for variable
            varName = varName.Trim().ToUpper();
            if (this.stringVariables.Keys.Contains(varName) == true)
            {
                retVal = this.stringVariables[varName];
            }
            else
            {
                throw new RunTimeException(String.Format("Could not find string variable: {0}", varName));
            }

            return retVal;
        }

        /// <summary>
        /// Checks to see if a variable exists
        /// </summary>
        /// <param name="varName">the name of the variable</param>
        /// <returns>true if it exists</returns>
        public bool VarExists(string varName)
        {
            // var init
            Boolean retVal = false;

            // look for variable
            varName = varName.Trim().ToUpper();
            if (this.stringVariables.Keys.Contains(varName) == true)
            {
                retVal = true;
            }
            else if (this.numericVariables.Keys.Contains(varName) == true)
            {
                retVal = true;
            }

            return retVal;
        }

        /// <summary>
        /// Creates or updates a numeric variable
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <param name="val">the value</param>
        public void SetNumericVar(string varName, double val)
        {
            // add the variable if neccessary
            varName = varName.Trim().ToUpper();
            if (this.numericVariables.Keys.Contains(varName) == false)
            {
                this.numericVariables.Add(varName, 0);
            }

            // update the value
            this.numericVariables[varName] = val;
        }

        /// <summary>
        /// Creates or updates a string variable
        /// </summary>
        /// <param name="varName">the var name</param>
        /// <param name="val">the value</param>
        public void SetStringVar(string varName, string val)
        {
            // add the variable if neccessary
            varName = varName.Trim().ToUpper();
            if (this.stringVariables.Keys.Contains(varName) == false)
            {
                this.stringVariables.Add(varName, String.Empty);
            }

            // update the value
            this.stringVariables[varName] = val;
        }

        /// <summary>
        /// Translates points from zero-centered to .NET image box style (if
        /// ExternalInterface contains a valid image to draw to), otherwise
        /// it just returns the point as is
        /// </summary>
        /// <param name="p">zero centered point</param>
        /// <returns>point</returns>
        public Point TranslateZeroCenteredPointToNET(Point p)
        {
            // if no image in ExternalInterface, just return the point
            if (this.ExternalInterface.GraphicsOutput == null)
            {
                return p;
            }
            
            // translate the point
            return new Point(Convert.ToInt32(.5 * this.ExternalInterface.GraphicsOutput.Size.Width) + p.X,
                             Convert.ToInt32(.5 * this.ExternalInterface.GraphicsOutput.Size.Height) - p.Y);
        }
    }
}
