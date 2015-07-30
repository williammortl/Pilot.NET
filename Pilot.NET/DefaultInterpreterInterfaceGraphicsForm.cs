namespace Pilot.NET
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    
    /// <summary>
    /// Window to display PILOT graphics
    /// </summary>
    internal partial class DefaultInterpreterInterfaceGraphicsForm : Form
    {

        /// <summary>
        /// the image containing the graphics, not responsible for the disposal of this
        /// </summary>
        private Image graphicsImage;

        /// <summary>
        /// Private constructor, use ShowForm method instead
        /// </summary>
        /// <param name="graphicsImage">the image to draw</param>
        /// <param name="title">the title for the window</param>
        private DefaultInterpreterInterfaceGraphicsForm(Image graphicsImage, String title)
        {
            this.graphicsImage = graphicsImage;
            this.Text = title;
            InitializeComponent();
        }

        /// <summary>
        /// Form load event
        /// </summary>
        /// <param name="sender">who triggered the event</param>
        /// <param name="e">event args</param>
        private void DefaultInterpreterInterfaceGraphicsForm_Load(object sender, EventArgs e)
        {

            // form init
            this.Height = this.graphicsImage.Height;
            this.Width = this.graphicsImage.Width;
            this.graphicsBox.Height = this.graphicsImage.Height;
            this.graphicsBox.Width = this.graphicsImage.Width;
        }

        /// <summary>
        /// Picture box repaint event
        /// </summary>
        /// <param name="sender">who triggered the event</param>
        /// <param name="e">event args</param>
        private void graphicsBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(this.graphicsImage, new Point(0, 0));   
        }

        /// <summary>
        /// Form repaint event
        /// </summary>
        /// <param name="sender">who triggered the event</param>
        /// <param name="e">event args</param>
        private void DefaultInterpreterInterfaceGraphicsForm_Paint(object sender, PaintEventArgs e)
        {
            this.graphicsBox.Invalidate();
        }

        /// <summary>
        /// Creates and shows a DefaultInterpreterInterfaceGraphicsForm form
        /// </summary>
        /// <param name="graphicsImage">the image to be drawn</param>
        /// <param name="title">the title of the window</param>
        /// <returns>a DefaultInterpreterInterfaceGraphicsForm variable</returns>
        public static DefaultInterpreterInterfaceGraphicsForm ShowForm(Image graphicsImage, String title)
        {
            DefaultInterpreterInterfaceGraphicsForm frm = new DefaultInterpreterInterfaceGraphicsForm(graphicsImage, title);
            Thread t = new Thread(new ParameterizedThreadStart(DefaultInterpreterInterfaceGraphicsForm.MessagePump));
            t.Start(frm);
            while (frm.IsHandleCreated == false)
            {
                Thread.Sleep(0);
            }
            return frm;
        }

        /// <summary>
        /// Message pump method for the form
        /// </summary>
        /// <param name="obj">the DefaultInterpreterInterfaceGraphicsForm that this a pump for</param>
        private static void MessagePump(Object obj)
        {
            DefaultInterpreterInterfaceGraphicsForm frm = (DefaultInterpreterInterfaceGraphicsForm)obj;
            Application.Run(frm);
            frm.Dispose();
        }
    }
}
