namespace PILOTi
{
    using System;
    using System.Windows.Forms;
    
    /// <summary>
    /// Splash form
    /// </summary>
    public partial class Splash : Form
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public Splash()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Timer to close the form
        /// </summary>
        /// <param name="sender">who triggered the event</param>
        /// <param name="e">event args</param>
        private void closeTimer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
