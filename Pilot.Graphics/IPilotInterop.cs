namespace Pilot.Graphics
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Provides a way for the PILOT interpreter to interoperate with users
    /// </summary>
    public interface IPilotInterop : IDisposable
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
        /// Clears the graphics
        /// </summary>
        void ClearGraphics();

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
        /// Close the graphics windows
        /// </summary>
        void CloseGraphicsWindow();

        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="frequency">frequency of sound</param>
        /// <param name="playMilliseconds">sound duration</param>
        void PlaySound(double frequency, int playMilliseconds);

        /// <summary>
        /// Translates points from zero-centered PILOT coordinates
        /// to whatever the system is for GraphicsOutput
        /// </summary>
        /// <param name="p">zero centered point</param>
        /// <returns>point</returns>
        Point TranslateCoordinates(Point p);
    }
}
