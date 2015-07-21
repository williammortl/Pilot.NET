namespace Pilot.NET.Interpreter.InterpreterInterfaces
{
    partial class DefaultInterpreterInterfaceGraphicsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.graphicsBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.graphicsBox)).BeginInit();
            this.SuspendLayout();
            // 
            // graphicsBox
            // 
            this.graphicsBox.BackColor = System.Drawing.Color.Black;
            this.graphicsBox.Location = new System.Drawing.Point(1, -2);
            this.graphicsBox.Name = "graphicsBox";
            this.graphicsBox.Size = new System.Drawing.Size(504, 418);
            this.graphicsBox.TabIndex = 0;
            this.graphicsBox.TabStop = false;
            this.graphicsBox.Paint += new System.Windows.Forms.PaintEventHandler(this.graphicsBox_Paint);
            // 
            // DefaultInterpreterInterfaceGraphicsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 607);
            this.Controls.Add(this.graphicsBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefaultInterpreterInterfaceGraphicsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.DefaultInterpreterInterfaceGraphicsForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DefaultInterpreterInterfaceGraphicsForm_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.graphicsBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox graphicsBox;
    }
}