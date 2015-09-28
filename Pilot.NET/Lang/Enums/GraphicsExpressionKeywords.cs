namespace Pilot.NET.Lang.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Graphics expression keywords
    /// </summary>
    public enum GraphicsExpressionKeywords
    {

        /// <summary>
        /// Clears the graphics
        /// </summary>
        [Description("CLEARS THE SCREEN")]
        CLEAR,

        /// <summary>
        /// Draws
        /// </summary>
        [Description("DRAWS THIS AMOUNT FORWARD")]
        DRAW,

        /// <summary>
        /// Draws
        /// </summary>
        [Description("DRAWS A LINE TO THIS POINT")]
        DRAWTO,

        /// <summary>
        /// Fills
        /// </summary>
        [Description("GO THIS AMOUNT FORWARD AND FILLS TO THE RIGHT")]
        FILL,

        /// <summary>
        /// Fills
        /// </summary>
        [Description("GOES TO THE POINT AND FILLS TO THE RIGHT")]
        FILLTO,

        /// <summary>
        /// Moves the turtle
        /// </summary>
        [Description("GO THIS AMOUNT FORWARD AND PLOT")]
        GO,

        /// <summary>
        /// Moves the turtle
        /// </summary>
        [Description("GO TO POINT AND PLOT")]
        GOTO,

        /// <summary>
        /// Changes the pen color
        /// </summary>
        [Description("CHANGES THE COLOR")]
        PEN,

        /// <summary>
        /// Turns the turls
        /// </summary>
        [Description("TURN THIS AMOUNT")]
        TURN,

        /// <summary>
        /// Turns the turtle
        /// </summary>
        [Description("TURN TO ANGLE")]
        TURNTO,

        /// <summary>
        /// Quits the graphics
        /// </summary>
        [Description("CLOSES THE GRAPHICS")]
        QUIT
    }
}
