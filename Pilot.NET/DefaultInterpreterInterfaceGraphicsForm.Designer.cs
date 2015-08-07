namespace Pilot.NET
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
            this.saveButton = new System.Windows.Forms.Button();
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
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.Blue;
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.saveButton.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.Location = new System.Drawing.Point(0, 535);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(1168, 72);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save Image >>";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // DefaultInterpreterInterfaceGraphicsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 607);
            this.Controls.Add(this.saveButton);
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
        private System.Windows.Forms.Button saveButton;
    }
}