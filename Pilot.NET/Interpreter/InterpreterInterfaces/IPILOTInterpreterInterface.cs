namespace Pilot.NET.Interpreter.InterpreterInterfaces
{
    using System;
    using System.Windows.Forms;
    
    /// <summary>
    /// Provides a way for the PILOT interpreter to read/write text and output graphics
    /// </summary>
    public interface IPILOTInterpreterInterface : IDisposable
    {

        /// <summary>
        /// This is the PictureBox that the PILOT translator will output graphics to, 
        /// if null this means text only, and the PILOT interpreter will not draw graphics,
        /// it will just skip the statements
        /// </summary>
        PictureBox GraphicsOutput { get; }

        /// <summary>
        /// Called by the PILOT translator to write text to the screen
        /// </summary>
        /// <param name="text">the text to write</param>
        void WriteText(String text);

        /// <summary>
        /// Called by the PILOT translator to prompt the user for input
        /// </summary>
        /// <returns>a line of text from the user</returns>
        String ReadText();
    }
}
