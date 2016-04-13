namespace Pilot.NET
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    
    /// <summary>
    /// Provides a way for the PILOT interpreter to read/write text and output graphics
    /// </summary>
    public interface IPILOTInterpreterInterface : IDisposable
    {

        /// <summary>
        /// This is the Image that the PILOT translator will output graphics to, 
        /// if null this means only text, and the PILOT interpreter will not draw graphics,
        /// it will just skip the statements
        /// </summary>
        Image GraphicsOutput { get; }

        /// <summary>
        /// After any new graphics are drawn on the image, call this to redraw the graphics
        /// </summary>
        void RedrawGraphics();

        /// <summary>
        /// Called by the PILOT translator to write text to the screen
        /// </summary>
        /// <param name="text">the text to write</param>
        /// <param name="lineBreak">add a line break</param>
        void WriteText(String text, Boolean lineBreak);

        /// <summary>
        /// Called by the PILOT translator to prompt the user for input, reads a whole line
        /// </summary>
        /// <returns>a line of text from the user</returns>
        String ReadTextLine();

        /// <summary>
        /// Clears the text screen
        /// </summary>
        void ClearText();

        /// <summary>
        /// Clears the graphics
        /// </summary>
        void ClearGraphics();

        /// <summary>
        /// Close the graphics windows
        /// </summary>
        void CloseGraphicsWindow();
    }
}
