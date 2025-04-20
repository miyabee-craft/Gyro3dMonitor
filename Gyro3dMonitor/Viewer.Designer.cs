namespace Gyro3dMonitor
{
    partial class Viewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Viewer));
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel_StateMonitor = new ToolStripStatusLabel();
            toolStripStatusLabel_Angle = new ToolStripStatusLabel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            statusStrip2 = new StatusStrip();
            toolStripStatusLabel_Zoom = new ToolStripStatusLabel();
            toolStripStatusLabel_Eye = new ToolStripStatusLabel();
            toolStripStatusLabel_Rotate = new ToolStripStatusLabel();
            statusStrip1.SuspendLayout();
            statusStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel_StateMonitor, toolStripStatusLabel_Angle, toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 439);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(484, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_StateMonitor
            // 
            toolStripStatusLabel_StateMonitor.BackColor = Color.Red;
            toolStripStatusLabel_StateMonitor.ForeColor = Color.White;
            toolStripStatusLabel_StateMonitor.Name = "toolStripStatusLabel_StateMonitor";
            toolStripStatusLabel_StateMonitor.Size = new Size(53, 17);
            toolStripStatusLabel_StateMonitor.Text = " オフライン";
            // 
            // toolStripStatusLabel_Angle
            // 
            toolStripStatusLabel_Angle.Name = "toolStripStatusLabel_Angle";
            toolStripStatusLabel_Angle.Size = new Size(90, 17);
            toolStripStatusLabel_Angle.Text = "Roll, Pitch, Yaw:";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(295, 17);
            toolStripStatusLabel1.Spring = true;
            // 
            // statusStrip2
            // 
            statusStrip2.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel_Zoom, toolStripStatusLabel_Eye, toolStripStatusLabel_Rotate });
            statusStrip2.Location = new Point(0, 417);
            statusStrip2.Name = "statusStrip2";
            statusStrip2.Size = new Size(484, 22);
            statusStrip2.SizingGrip = false;
            statusStrip2.TabIndex = 1;
            statusStrip2.Text = "statusStrip2";
            // 
            // toolStripStatusLabel_Zoom
            // 
            toolStripStatusLabel_Zoom.Name = "toolStripStatusLabel_Zoom";
            toolStripStatusLabel_Zoom.Size = new Size(27, 17);
            toolStripStatusLabel_Zoom.Text = "Zm:";
            // 
            // toolStripStatusLabel_Eye
            // 
            toolStripStatusLabel_Eye.Name = "toolStripStatusLabel_Eye";
            toolStripStatusLabel_Eye.Size = new Size(28, 17);
            toolStripStatusLabel_Eye.Text = "Eye:";
            // 
            // toolStripStatusLabel_Rotate
            // 
            toolStripStatusLabel_Rotate.Name = "toolStripStatusLabel_Rotate";
            toolStripStatusLabel_Rotate.Size = new Size(71, 17);
            toolStripStatusLabel_Rotate.Text = "Rot (X, Y, Z):";
            // 
            // Viewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 461);
            Controls.Add(statusStrip2);
            Controls.Add(statusStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Viewer";
            Text = "Viewer";
            FormClosing += Viewer_FormClosing;
            Load += Viewer_Load;
            MouseDown += Viewer_MouseDown;
            MouseUp += Viewer_MouseUp;
            Resize += Viewer_Resize;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            statusStrip2.ResumeLayout(false);
            statusStrip2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel_Angle;
        private ToolStripStatusLabel toolStripStatusLabel_StateMonitor;
        private StatusStrip statusStrip2;
        private ToolStripStatusLabel toolStripStatusLabel_Eye;
        private ToolStripStatusLabel toolStripStatusLabel_Zoom;
        private ToolStripStatusLabel toolStripStatusLabel_Rotate;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}